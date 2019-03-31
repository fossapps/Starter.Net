using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starter.Net.Api.Models;

namespace Starter.Net.Api.Services
{
    public interface IUserService
    {
        Task<(SignInResult signInResult, User user)> AuthenticateByUsername(string username, string password);
        Task<(SignInResult signInResult, User user)> AuthenticateByEmail(string email, string password);
        Task<(SignInResult signInResult, User user)> Authenticate(string login, string password);
    }
}
