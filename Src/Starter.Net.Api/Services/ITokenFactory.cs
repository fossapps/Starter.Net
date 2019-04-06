namespace Starter.Net.Api.Services
{
    public interface ITokenFactory
    {
        string GenerateToken(int size);
    }
}
