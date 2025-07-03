using System.ComponentModel.DataAnnotations;

namespace Evernet.WebApi.Entities;

public class RefreshToken
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required] [MaxLength(256)] public required string Token { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public bool IsRevoked => RevokedAt.HasValue || DateTime.UtcNow >= ExpiresAt;

    public Guid UserId { get; set; }

    public required User User { get; set; }
}