namespace Evernet.WebApi.Interfaces;

public interface ICodeGenerator
{
    string GenerateNumericCode(int length = 6);
}