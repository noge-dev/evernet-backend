using System.ComponentModel.DataAnnotations;

namespace Evernet.WebApi.DTOs;

public sealed record ChangePasswordDto(string CurrentPassword, string NewPassword);