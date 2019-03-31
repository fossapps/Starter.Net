using System.Threading.Tasks;
using Starter.Net.Api.Models;

namespace Starter.Net.Api.Repositories
{
    public interface IUsersRepository
    {
        Task<User> FindByNameAsync(string username);
        Task<User> FindByEmailAsync(string email);
    }
}
