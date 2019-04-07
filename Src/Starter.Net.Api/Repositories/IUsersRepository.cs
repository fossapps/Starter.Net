using System.Threading.Tasks;
using Starter.Net.Api.Models;

namespace Starter.Net.Api.Repositories
{
    public interface IUsersRepository
    {
        Task<User> Find(string emailOrUsername);
        Task<User> FindByNameAsync(string username);
        Task<User> FindByUserIdAsync(string userId);
        Task<User> FindByEmailAsync(string email);
    }
}
