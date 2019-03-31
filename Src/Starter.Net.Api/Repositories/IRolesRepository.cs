using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Starter.Net.Api.Repositories
{
    public interface IRolesRepository
    {
        Task<IList<string>> GetRolesByUserId(string userId);
        Task<IList<string>> GetRolesByUser(IdentityUser user);
        Task<IdentityRole> FindRoleByIdAsync(string roleId);
        Task<IdentityRole> FindRoleByNameAsync(string roleName);
    }
}
