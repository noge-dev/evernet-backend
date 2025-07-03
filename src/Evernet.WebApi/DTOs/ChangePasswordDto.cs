namespace Evernet.WebApi.DTOs;

public record ChangePasswordDto(Guid UserId, string CurrentPassword, string NewPassword);