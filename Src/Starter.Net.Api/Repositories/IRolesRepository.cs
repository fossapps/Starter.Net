using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starter.Net.Api.Models;

namespace Starter.Net.Api.Repositories
{
    public interface IRolesRepository
    {
        Task<IList<string>> GetRolesByUserId(string userId);
        Task<IList<string>> GetRolesByUser(Models.User user);
        Task<IdentityRole> FindRoleByIdAsync(string roleId);
        Task<IdentityRole> FindRoleByNameAsync(string roleName);
    }
}
