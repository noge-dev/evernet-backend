namespace Evernet.WebApi.DTOs;

public sealed record ResetPasswordDto(string Token, string NewPassword);