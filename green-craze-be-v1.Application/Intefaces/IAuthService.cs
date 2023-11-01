using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Auth;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IAuthService
    {
        Task<AuthDto> Authenticate(LoginRequest request);

        Task<AuthDto> AuthenticateWithGoogle(GoogleAuthRequest request);

        Task<string> Register(RegisterRequest request);

        Task<AuthDto> RefreshToken(RefreshTokenRequest request);

        Task RevokeRefreshToken(string userId);

        Task RevokeAllRefreshToken();
    }
}