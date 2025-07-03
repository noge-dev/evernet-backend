using System.Security.Cryptography;
using Evernet.WebApi.Interfaces;

namespace Evernet.WebApi.Services;

public class CodeGenerator : ICodeGenerator
{
    public string GenerateNumericCode(int length)
    {
        var digits = new char[length];
        var randomBytes = new byte[length];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        for (int i = 0; i < length; i++)
        {
            int digit = randomBytes[i] % 10;
            digits[i] = (char)('0' + digit);
        }

        return new string(digits);
    }
}