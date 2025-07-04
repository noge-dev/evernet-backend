using System.ComponentModel.DataAnnotations;

namespace Evernet.WebApi.DTOs;

public sealed record VerifyCodeDto(Guid UserId, string Code);