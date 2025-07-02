using System.ComponentModel.DataAnnotations;

namespace Evernet.WebApi.Entities;

public class VerificationCode
{
    public Guid Id { get; init; } = Guid.NewGuid();
    [Required] public required Guid UserId { get; init; }

    [Required, MaxLength(6)] public required string Code { get; init; }

    [Required] public required DateTime Expiration { get; init; }

    [Required] public required User User { get; set; }
}