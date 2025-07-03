namespace Evernet.WebApi.DTOs;

public sealed record LoginResponseDto(string AccessToken, string RefreshToken);