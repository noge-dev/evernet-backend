using System.Security.Claims;
using Evernet.WebApi.DTOs;
using Evernet.WebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evernet.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        await authService.RegisterAsync(dto);
        return Ok(new { Message = "Inscription réussie. Veuillez vérifier votre e-mail pour valider votre compte." });
    }

    [HttpPost("verify-code")]
    public async Task<IActionResult> Verify([FromBody] VerifyCodeDto dto)
    {
        await authService.VerifyCodeAsync(dto);
        return Ok(new { Message = "Vérification réussie. Vous pouvez maintenant vous connecter." });
    }

    [HttpPost("resend-code")]
    public async Task<IActionResult> ResendCode([FromBody] ResendCodeDto dto)
    {
        await authService.ResendCodeAsync(dto);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await authService.LoginAsync(dto);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<LoginResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto dto)
    {
        var result = await authService.RefreshTokenAsync(dto);
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto dto)
    {
        await authService.LogoutAsync(dto);
        return NoContent();
    }

    [HttpPost("request-reset-password")]
    public async Task<IActionResult> RequestResetPassword([FromBody] RequestResetPasswordDto dto)
    {
        await authService.RequestResetPasswordAsync(dto.Email);
        return Ok(new
            { Message = "Un e-mail vous a été envoyé avec les instructions de réinitialisation de mot de passe." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        await authService.ResetPasswordAsync(dto);
        return Ok(new
        {
            Message =
                "Mot de passe réinitialisé avec succès. Vous pouvez maintenant vous connecter avec votre nouveau mot de passe."
        });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                          ?? User.FindFirst("sub");

        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        await authService.ChangePasswordAsync(userId, dto);
        return Ok(new { Message = "Mot de passe modifié avec succès." });
    }
}