using System.ComponentModel.DataAnnotations;

namespace Evernet.WebApi.DTOs;

public class RegisterDto
{
    [Required, MaxLength(100), MinLength(1)]
    public required string FirstName { get; set; }

    [MaxLength(100)] public string? MiddleName { get; set; }

    [Required, MaxLength(100), MinLength(1)]
    public required string LastName { get; set; }

    [Required, EmailAddress, MaxLength(256)]
    public required string Email { get; set; }

    [Required, MinLength(6), MaxLength(100)]
    public required string Password { get; set; }
}