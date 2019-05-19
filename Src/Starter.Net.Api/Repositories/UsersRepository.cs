using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Starter.Net.Api.Models;
using Starter.Net.Api.ViewModels;

namespace Starter.Net.Api.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<Models.User> _userManager;

        public UsersRepository(UserManager<Models.User> userManager)
        {
            _userManager = userManager;
        }

        public Task<Models.User> Find(string emailOrUsername)
        {
            return emailOrUsername.Contains("@")
                ? FindByEmailAsync(emailOrUsername)
                : FindByNameAsync(emailOrUsername);
        }

        public Task<Models.User> FindByNameAsync(string username)
        {
            return _userManager.FindByNameAsync(username);
        }

        public Task<Models.User> FindByUserIdAsync(string userId)
        {
            return _userManager.FindByIdAsync(userId);
        }

        public Task<Models.User> FindByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public async Task<(IdentityResult result, UserRegistrationSuccessResponse user, string activationToken)> Create(Models.User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return (result, null, null);
            }
            var response = new UserRegistrationSuccessResponse()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName
            };
            var activationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return (result, response, activationToken);
        }
    }
}
