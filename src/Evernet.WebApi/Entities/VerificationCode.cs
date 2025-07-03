using System.ComponentModel.DataAnnotations;

namespace Evernet.WebApi.Entities;

public class VerificationCode
{
    public Guid Id { get; init; } = Guid.NewGuid();
    [Required] public required Guid UserId { get; init; }

    [Required, MaxLength(256)] public required string CodeHash { get; init; }

    [Required] public required DateTime Expiration { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    [Required] public required User User { get; set; }
}