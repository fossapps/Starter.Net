using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starter.Net.Api.Models;
using Starter.Net.Api.Repositories;

namespace Starter.Net.Api.Services
{
    public class UserService: IUserService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUsersRepository _usersRepository;

        public UserService(SignInManager<User> signInManager, IUsersRepository usersRepository)
        {
            _signInManager = signInManager;
            _usersRepository = usersRepository;
        }

        public Task<(SignInResult signInResult, ClaimsPrincipal principal, User user)> Authenticate(string login, string password)
        {
            return login.Contains("@")
                ? AuthenticateByEmail(login, password)
                : AuthenticateByUsername(login, password);
        }

        public async Task<(SignInResult signInResult, ClaimsPrincipal principal, User user)> AuthenticateByUsername(string username, string password)
        {
            var user = await _usersRepository.FindByNameAsync(username);
            return await AuthenticateUser(user, password);
        }

        public async Task<(SignInResult signInResult, ClaimsPrincipal principal, User user)> AuthenticateByEmail(string email, string password)
        {
            var user = await _usersRepository.FindByEmailAsync(email);
            return await AuthenticateUser(user, password);
        }

        private async Task<(SignInResult signInResult, ClaimsPrincipal principal, User user)> AuthenticateUser(User user, string password)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, true);
            if (!signInResult.Succeeded)
            {
                return (signInResult, null, null);
            }
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            return (signInResult, principal, user);
        }
    }
}
