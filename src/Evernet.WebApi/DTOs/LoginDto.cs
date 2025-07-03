using System.ComponentModel.DataAnnotations;

namespace Evernet.WebApi.DTOs;

public class LoginDto
{
    [Required] [EmailAddress] public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}