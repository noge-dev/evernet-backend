namespace Evernet.WebApi.DTOs;

public record ResetPasswordDto(string Token, string NewPassword);