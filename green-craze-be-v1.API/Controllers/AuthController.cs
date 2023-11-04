using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Auth;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICurrentUserService _currentUserService;

        public AuthController(IAuthService authService, ICurrentUserService currentUserService)
        {
            _authService = authService;
            _currentUserService = currentUserService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            var authResp = await _authService.Authenticate(request);

            return Ok(new APIResponse<AuthDto>(authResp, StatusCodes.Status200OK));
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> AuthenticateWithGoogle([FromBody] GoogleAuthRequest request)
        {
            var authResp = await _authService.AuthenticateWithGoogle(request);

            return Ok(new APIResponse<AuthDto>(authResp, StatusCodes.Status200OK));
        }

        [HttpPut("register/verify")]
        public async Task<IActionResult> VerifyRegistation([FromBody] VerifyOTPRequest request)
        {
            request.Type = TOKEN_TYPE.REGISTER_OTP;
            var isSuccess = await _authService.VerifyOTP(request);

            return Ok(new APIResponse<bool>(isSuccess, StatusCodes.Status204NoContent));
        }

        [HttpPut("register/resend")]
        public async Task<IActionResult> ResendRegistationOTP([FromQuery] string email)
        {
            var isSuccess = await _authService.ResendOTP(email, TOKEN_TYPE.REGISTER_OTP);

            return Ok(new APIResponse<bool>(isSuccess, StatusCodes.Status204NoContent));
        }

        [HttpPut("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var isSuccess = await _authService.ForgotPassword(email);

            return Ok(new APIResponse<bool>(isSuccess, StatusCodes.Status204NoContent));
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var isSuccess = await _authService.ResetPassword(request);

            return Ok(new APIResponse<bool>(isSuccess, StatusCodes.Status204NoContent));
        }

        [HttpPut("forgot-password/verify")]
        public async Task<IActionResult> VerifyForgottenPassword([FromBody] VerifyOTPRequest request)
        {
            request.Type = TOKEN_TYPE.FORGOT_PASSWORD_OTP;
            var isSuccess = await _authService.VerifyOTP(request);

            return Ok(new APIResponse<bool>(isSuccess, StatusCodes.Status204NoContent));
        }

        [HttpPut("forgot-password/resend")]
        public async Task<IActionResult> ResendForgottenPasswordOTP([FromQuery] string email)
        {
            var isSuccess = await _authService.ResendOTP(email, TOKEN_TYPE.FORGOT_PASSWORD_OTP);

            return Ok(new APIResponse<bool>(isSuccess, StatusCodes.Status204NoContent));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var userId = await _authService.Register(request);

            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/users/{userId}";

            return Created(url, new APIResponse<object>(new { id = userId }, StatusCodes.Status201Created));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var authResp = await _authService.RefreshToken(request);

            return Ok(new APIResponse<AuthDto>(authResp, StatusCodes.Status200OK));
        }

        [HttpPost("revoke-refresh-token")]
        [Authorize]
        public async Task<IActionResult> RevokeRefreshToken()
        {
            await _authService.RevokeRefreshToken(_currentUserService.UserId);

            return Ok(new APIResponse<bool>(true, StatusCodes.Status200OK));
        }
    }
}