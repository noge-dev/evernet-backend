using System.Security.Cryptography;
using Evernet.WebApi.Interfaces;

namespace Evernet.WebApi.Services;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate(int length = 64)
    {
        var bytes = RandomNumberGenerator.GetBytes(length);
        return Convert.ToBase64String(bytes);
    }
}