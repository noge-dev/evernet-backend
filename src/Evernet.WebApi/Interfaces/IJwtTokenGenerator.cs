namespace Evernet.WebApi.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email);
}