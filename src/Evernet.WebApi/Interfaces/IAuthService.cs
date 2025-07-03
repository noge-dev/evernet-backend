using Evernet.WebApi.DTOs;
using Evernet.WebApi.Entities;

namespace Evernet.WebApi.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto dto);
    Task VerifyCodeAsync(VerifyCodeDto dto);
    Task ResendCodeAsync(ResendCodeDto dto);
    Task<LoginResponseDto> LoginAsync(LoginDto dto);
    Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto);
    Task LogoutAsync(LogoutRequestDto dto);
    Task RequestResetPasswordAsync(string email);
    Task ResetPasswordAsync(ResetPasswordDto dto);
}