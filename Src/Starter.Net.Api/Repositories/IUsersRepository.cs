using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starter.Net.Api.Models;
using Starter.Net.Api.ViewModels;

namespace Starter.Net.Api.Repositories
{
    public interface IUsersRepository
    {
        Task<Models.User> Find(string emailOrUsername);
        Task<Models.User> FindByNameAsync(string username);
        Task<Models.User> FindByUserIdAsync(string userId);
        Task<Models.User> FindByEmailAsync(string email);
        Task<(IdentityResult result, UserRegistrationSuccessResponse user, string activationToken)> Create(Models.User user, string password);
    }
}
