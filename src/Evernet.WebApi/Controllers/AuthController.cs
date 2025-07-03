using Evernet.WebApi.DTOs;
using Evernet.WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Evernet.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            await authService.RegisterAsync(dto);
            return Ok(new { Message = "Registration successful. Please check your email for verification." });
        }
        catch (Exception e)
        {
            return BadRequest(new { e.Message });
        }
    }

    [HttpPost("verify-code")]
    public async Task<IActionResult> Verify([FromBody] VerifyCodeDto dto)
    {
        try
        {
            await authService.VerifyCodeAsync(dto);
            return Ok(new { Message = "Verification successful. You can now log in." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
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
}