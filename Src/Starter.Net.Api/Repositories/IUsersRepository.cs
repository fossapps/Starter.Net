using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starter.Net.Api.Models;
using Starter.Net.Api.ViewModels;

namespace Starter.Net.Api.Repositories
{
    public interface IUsersRepository
    {
        Task<User> Find(string emailOrUsername);
        Task<User> FindByNameAsync(string username);
        Task<User> FindByUserIdAsync(string userId);
        Task<User> FindByEmailAsync(string email);
        Task<(IdentityResult result, UserRegistrationSuccessResponse user, string activationToken)> Create(User user, string password);
    }
}
