using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starter.Net.Api.Models;
using Starter.Net.Api.ViewModels;

namespace Starter.Net.Api.Services
{
    public interface IUserService
    {
        Task<(SignInResult signInResult, ClaimsPrincipal principal, User user)> AuthenticateByUsername(string username, string password);
        Task<(SignInResult signInResult, ClaimsPrincipal principal, User user)> AuthenticateByEmail(string email, string password);
        Task<(SignInResult signInResult, ClaimsPrincipal principal, User user)> Authenticate(string login, string password);
        Task<RefreshTokenResponse> RefreshAuthentication(RefreshToken token);
    }
}
