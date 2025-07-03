using System.ComponentModel.DataAnnotations;

namespace Evernet.WebApi.DTOs;

public class VerifyCodeDto
{
    [Required] public required Guid UserId { get; set; }

    [Required, StringLength(6, MinimumLength = 6)]
    public required string Code { get; set; }
}