using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Starter.Net.Api.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RolesRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IList<string>> GetRolesByUserId(string userId)
        {
            return await GetRolesByUser(await _userManager.FindByIdAsync(userId));
        }

        public async Task<IList<string>> GetRolesByUser(IdentityUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
