using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Starter.Net.Api.Models;

namespace Starter.Net.Api.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IRoleStore<IdentityRole> _roleStore;

        public RolesRepository(UserManager<User> userManager, IRoleStore<IdentityRole> roleStore)
        {
            _userManager = userManager;
            _roleStore = roleStore;
        }

        public async Task<IList<string>> GetRolesByUserId(string userId)
        {
            return await GetRolesByUser(await _userManager.FindByIdAsync(userId));
        }

        public async Task<IdentityRole> FindRoleByIdAsync(string roleId)
        {
            return await _roleStore.FindByIdAsync(roleId, CancellationToken.None);
        }

        public Task<IdentityRole> FindRoleByNameAsync(string roleName)
        {
            return _roleStore.FindByNameAsync(roleName, CancellationToken.None);
        }

        public async Task<IList<string>> GetRolesByUser(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
