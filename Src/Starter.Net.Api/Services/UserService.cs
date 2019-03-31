using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starter.Net.Api.Models;

namespace Starter.Net.Api.Services
{
    public class UserService: IUserService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public UserService(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public Task<(SignInResult signInResult, User user)> Authenticate(string login, string password)
        {
            return login.Contains("@")
                ? AuthenticateByEmail(login, password)
                : AuthenticateByUsername(login, password);
        }

        public async Task<(SignInResult signInResult, User user)> AuthenticateByUsername(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await AuthenticateUser(user, password);
        }

        public async Task<(SignInResult signInResult, User user)> AuthenticateByEmail(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return await AuthenticateUser(user, password);
        }

        private async Task<(SignInResult signInResult, User user)> AuthenticateUser(User user, string password)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, true);
            return (signInResult, user);
        }
    }
}
