using System.ComponentModel.DataAnnotations;

namespace Evernet.WebApi.DTOs;

public sealed record RegisterDto(
    string FirstName,
    string? MiddleName,
    string LastName,
    string Email,
    string Password
);