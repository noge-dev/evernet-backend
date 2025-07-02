using System.ComponentModel.DataAnnotations;

namespace Evernet.WebApi.Entities;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required, MaxLength(100), MinLength(1)]
    public required string FirstName { get; set; }

    [MaxLength(100), MinLength(1)] public string? MiddleName { get; set; }

    [Required, MaxLength(100), MinLength(1)]
    public required string LastName { get; set; }

    [Required, MaxLength(256), EmailAddress]
    public required string Email { get; set; }

    [Required, MaxLength(256)] public required string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public ICollection<VerificationCode> VerificationCodes { get; set; } = new List<VerificationCode>();
}