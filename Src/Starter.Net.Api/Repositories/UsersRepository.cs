using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starter.Net.Api.Models;

namespace Starter.Net.Api.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;

        public UsersRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public Task<User> FindByNameAsync(string username)
        {
            return _userManager.FindByNameAsync(username);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }
    }
}