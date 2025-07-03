using System.ComponentModel.DataAnnotations;

namespace Evernet.WebApi.DTOs;

public record ChangePasswordDto(
    [Required] string CurrentPassword,
    [Required] [MinLength(8)] string NewPassword
);