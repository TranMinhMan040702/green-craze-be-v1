using AutoMapper;
using Google.Apis.Auth;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Auth;
using green_craze_be_v1.Application.Specification.User;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly ITokenService _tokenService;
        private readonly IMailService _mailService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IJwtService jwtService,
            IMapper mapper,
            IDateTimeService dateTimeService,
            ICurrentUserService currentUserService,
            ITokenService tokenService,
            IMailService mailService,
            IUnitOfWork unitOfWork)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
            _tokenService = tokenService;
            _mailService = mailService;
            _unitOfWork = unitOfWork;
        }

        private async Task<AuthDto> GenerateAuthCredential(AppUser user)
        {
            string accessToken = await _jwtService.CreateJWT(user.Id);
            string refreshToken = _jwtService.CreateRefreshToken();
            DateTime refreshTokenExpiredTime = DateTime.Now.AddDays(7);

            var u = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(new UserSpecification(user.Id))
                ?? throw new NotFoundException("Cannot find current user");

            var userToken = u.AppUserTokens.FirstOrDefault(x => x.Type == TOKEN_TYPE.REFRESH_TOKEN);
            if (userToken == null)
            {
                u.AppUserTokens.Add(new AppUserToken()
                {
                    Token = refreshToken,
                    ExpiredAt = refreshTokenExpiredTime,
                    Type = TOKEN_TYPE.REFRESH_TOKEN,
                    CreatedAt = _dateTimeService.Current,
                    CreatedBy = "System"
                });
            }
            else
            {
                userToken.Token = refreshToken;
                userToken.ExpiredAt = refreshTokenExpiredTime;
                userToken.UpdatedAt = _dateTimeService.Current;
                userToken.UpdatedBy = _currentUserService.UserId;
            }

            var isSuccess = await _userManager.UpdateAsync(u);
            if (!isSuccess.Succeeded)
                throw new Exception("Cannot login, please contact administrator");

            return new AuthDto { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public async Task<AuthDto> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email)
                    ?? throw new InvalidRequestException("Email is incorrect, cannot login");
            var res = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (res.IsLockedOut)
            {
                throw new AccessDeniedException("Your account has been lockout, unlock in " + user.LockoutEnd);
            }
            if (!res.Succeeded)
                throw new InvalidRequestException("Password is incorrect");

            if (user.Status == USER_STATUS.IN_ACTIVE)
                throw new AccessDeniedException("Your account has been banned");

            if (!user.EmailConfirmed)
                throw new AccessDeniedException("Your account hasn't been confirmed");

            return await GenerateAuthCredential(user);
        }

        private static async Task<GoogleJsonWebSignature.Payload> IsGoogleTokenValid(string token)
        {
            try
            {
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token);
                return payload;
            }
            catch
            {
                throw new InvalidRequestException("Google token is invalid, cannot login");
            }
        }

        public async Task<AuthDto> AuthenticateWithGoogle(GoogleAuthRequest request)
        {
            var payload = await IsGoogleTokenValid(request.GoogleToken);
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user != null)
            {
                if (user.Status == USER_STATUS.IN_ACTIVE)
                    throw new AccessDeniedException("Your account has been banned");
                user.EmailConfirmed = true;
                return await GenerateAuthCredential(user);
            }
            var registerRequest = new RegisterRequest()
            {
                Email = payload.Email,
                FirstName = payload.FamilyName,
                LastName = payload.GivenName,
                Password = payload.Subject
            };
            var userId = await Register(registerRequest, isGoogleAuthen: true);
            user = await _userManager.FindByIdAsync(userId);

            var resp = await GenerateAuthCredential(user);

            user.Avatar = payload.Picture;

            user.UpdatedAt = _dateTimeService.Current;
            user.UpdatedBy = _currentUserService.UserId;
            await _userManager.UpdateAsync(user);

            return resp;
        }

        public async Task<AuthDto> RefreshToken(RefreshTokenRequest request)
        {
            var userPrincipal = _jwtService.ValidateExpiredJWT(request.AccessToken)
                ?? throw new UnauthorizedException("Invalid access token");
            var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(new UserSpecification(userId))
                ?? throw new NotFoundException("Cannot find current user");

            var userToken = user.AppUserTokens.FirstOrDefault(x => x.Type == TOKEN_TYPE.REFRESH_TOKEN);
            if (user is null || userToken is null || userToken.Token != request.RefreshToken || userToken.ExpiredAt <= DateTime.Now)
            {
                throw new UnauthorizedException("Invalid access token or refresh token");
            }
            var newAccessToken = await _jwtService.CreateJWT(user.Id);
            var newRefreshToken = _jwtService.CreateRefreshToken();
            userToken.Token = newRefreshToken;
            userToken.UpdatedAt = _dateTimeService.Current;
            userToken.UpdatedBy = _currentUserService.UserId;
            await _userManager.UpdateAsync(user);

            return new AuthDto { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
        }

        public async Task<string> Register(RegisterRequest request, bool isGoogleAuthen = false)
        {
            var user = _mapper.Map<AppUser>(request);
            user.Status = USER_STATUS.ACTIVE;
            user.UserName = Regex.Replace(request.Email, "[^A-Za-z0-9 -]", "");
            user.CreatedAt = _dateTimeService.Current;
            user.CreatedBy = "System";
            user.Cart = new Cart();
            var res = await _userManager.CreateAsync(user, request.Password);

            if (res.Succeeded)
            {
                List<string> roles = new()
                {
                    USER_ROLE.USER
                };
                await _userManager.AddToRolesAsync(user, roles);
                if (!isGoogleAuthen)
                {
                    var otp = _tokenService.GenerateOTP();
                    user.AppUserTokens.Add(new AppUserToken()
                    {
                        Token = otp,
                        ExpiredAt = _dateTimeService.Current.AddMinutes(5),
                        Type = TOKEN_TYPE.REGISTER_OTP,
                        CreatedAt = _dateTimeService.Current,
                        CreatedBy = "System"
                    });

                    await _userManager.UpdateAsync(user);

                    var content = "Mã OTP: " + otp + "\nHiệu lực trong 5 phút";
                    var title = "Xác nhận đăng ký tài khoản";
                    var name = user.FirstName + " " + user.LastName;
                    _mailService.SendMail(name, user.Email, content, title);
                }

                return user.Id;
            }

            string error = "";
            res.Errors.ToList().ForEach(x => error += (x.Description + "/n"));
            throw new Exception(error);
        }

        public async Task RevokeAllRefreshToken()
        {
            var users = await _unitOfWork.Repository<AppUser>().ListAsync(new UserSpecification());
            foreach (var user in users)
            {
                var userToken = user.AppUserTokens.FirstOrDefault(x => x.Type == TOKEN_TYPE.REFRESH_TOKEN)
                    ?? throw new NotFoundException("Refresh token of user is not found");
                userToken.Token = null;
                userToken.ExpiredAt = null;
                userToken.UpdatedAt = _dateTimeService.Current;
                userToken.UpdatedBy = _currentUserService.UserId;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task RevokeRefreshToken(string userId)
        {
            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(new UserSpecification(userId))
                ?? throw new NotFoundException("Cannot find current user");
            var userToken = user.AppUserTokens.FirstOrDefault(x => x.Type == TOKEN_TYPE.REFRESH_TOKEN)
                ?? throw new NotFoundException("Refresh token of current user is not found");
            userToken.Token = null;
            userToken.ExpiredAt = null;
            userToken.UpdatedAt = _dateTimeService.Current;
            userToken.UpdatedBy = _currentUserService.UserId;
            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> VerifyOTP(VerifyOTPRequest request)
        {
            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(new UserSpecification(request.Email, true))
                ?? throw new NotFoundException("Cannot find user with email: " + request.Email);

            var userToken = user.AppUserTokens.FirstOrDefault(x => x.Type == request.Type);

            if (userToken is null || userToken.Token != request.OTP)
            {
                throw new InvalidRequestException("OTP is invalid");
            }

            if (userToken.ExpiredAt <= _dateTimeService.Current)
            {
                throw new InvalidRequestException("OTP is expired");
            }

            userToken.Token = null;
            userToken.ExpiredAt = null;
            if (request.Type == TOKEN_TYPE.REGISTER_OTP)
            {
                user.EmailConfirmed = true;
                user.UpdatedAt = _dateTimeService.Current;
                user.UpdatedBy = _currentUserService.UserId;
            }

            userToken.UpdatedAt = _dateTimeService.Current;
            userToken.UpdatedBy = _currentUserService.UserId;

            await _userManager.UpdateAsync(user);

            return true;
        }

        public async Task<bool> ResendOTP(string email, string type)
        {
            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(new UserSpecification(email, true))
                ?? throw new NotFoundException("Cannot find user with email: " + email);
            var userToken = user.AppUserTokens.FirstOrDefault(x => x.Type == type)
                ?? throw new InvalidRequestException("This user hasn't been signed up before, invalid request");

            var otp = _tokenService.GenerateOTP();

            userToken.Token = otp;
            userToken.ExpiredAt = _dateTimeService.Current.AddMinutes(5);
            userToken.UpdatedAt = _dateTimeService.Current;
            userToken.UpdatedBy = _currentUserService.UserId;

            await _userManager.UpdateAsync(user);

            var content = "Mã OTP: " + otp + "\nHiệu lực trong 5 phút";
            var title = "Gửi lại mã OTP";
            var name = user.FirstName + " " + user.LastName;
            _mailService.SendMail(name, user.Email, content, title);

            return true;
        }

        public async Task<bool> ForgotPassword(string email)
        {
            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(new UserSpecification(email, true))
                ?? throw new NotFoundException("Cannot find user with email: " + email);

            var userToken = user.AppUserTokens.FirstOrDefault(x => x.Type == TOKEN_TYPE.FORGOT_PASSWORD_OTP);

            var otp = _tokenService.GenerateOTP();
            if (userToken is null)
            {
                user.AppUserTokens.Add(new AppUserToken()
                {
                    Token = otp,
                    ExpiredAt = _dateTimeService.Current.AddMinutes(5),
                    Type = TOKEN_TYPE.FORGOT_PASSWORD_OTP,
                    CreatedAt = _dateTimeService.Current,
                    CreatedBy = "System"
                });
            }
            else
            {
                userToken.Token = otp;
                userToken.ExpiredAt = _dateTimeService.Current.AddMinutes(5);
                userToken.UpdatedAt = _dateTimeService.Current;
                userToken.UpdatedBy = _currentUserService.UserId;
            }
            await _userManager.UpdateAsync(user);

            var content = "Mã OTP: " + otp + "\nHiệu lực trong 5 phút";
            var title = "Quên mật khẩu";
            var name = user.FirstName + " " + user.LastName;
            _mailService.SendMail(name, user.Email, content, title);

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(new UserSpecification(request.Email, true))
                ?? throw new NotFoundException("Cannot find user with email: " + request.Email);

            var userToken = user.AppUserTokens.FirstOrDefault(x => x.Type == TOKEN_TYPE.FORGOT_PASSWORD_OTP)
                ?? throw new InvalidRequestException("Cannot reset password, this account has never receive request for forgotting password before");

            if (userToken.Token != null)
                throw new InvalidRequestException("Please verify OTP before reset password");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var res = await _userManager.ResetPasswordAsync(user, token, request.Password);

            return res.Succeeded;
        }
    }
}