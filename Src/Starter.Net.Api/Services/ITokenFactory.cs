using System.Security.Claims;

namespace Starter.Net.Api.Services
{
    public interface ITokenFactory
    {
        string GenerateToken(int size);
        string GenerateJwtToken(ClaimsPrincipal principal);
    }
}
