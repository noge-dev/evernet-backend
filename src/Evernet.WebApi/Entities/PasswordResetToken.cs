using System.ComponentModel.DataAnnotations;

namespace Evernet.WebApi.Entities;

public class PasswordResetToken
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(256)]
    public required string Token { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public required User User { get; set; }
}