namespace Evernet.WebApi.Interfaces;

public interface IRefreshTokenGenerator
{
    string Generate(int length = 64);
}