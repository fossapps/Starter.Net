using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starter.Net.Api.Models;

namespace Starter.Net.Api.Repositories
{
    public class ClaimsRepository : IClaimsRepository
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IRoleClaimStore<IdentityRole> _roleClaimStore;
        private readonly UserManager<User> _userManager;
        private readonly IUsersRepository _usersRepository;

        public ClaimsRepository(IRolesRepository rolesRepository, IRoleClaimStore<IdentityRole> roleClaimStore, UserManager<User> userManager, IUsersRepository usersRepository)
        {
            _rolesRepository = rolesRepository;
            _roleClaimStore = roleClaimStore;
            _userManager = userManager;
            _usersRepository = usersRepository;
        }

        public async Task<IList<Claim>> GetClaimsForRole(string roleName)
        {
            var role = await _rolesRepository.FindRoleByNameAsync(roleName);
            return await _roleClaimStore.GetClaimsAsync(role);
        }

        public async Task<IList<Claim>> GetClaimsForUserId(string userId)
        {
            return await GetClaimsForUser(await _usersRepository.FindByUserIdAsync(userId));
        }

        public async Task<IList<Claim>> GetClaimsForUser(User user)
        {
            var roles = await _rolesRepository.GetRolesByUserId(user.Id);
            var userClaims = (IEnumerable<Claim>) await _userManager.GetClaimsAsync(user);
            foreach (var role in roles)
            {
                var claims = await GetClaimsForRole(role);
                userClaims = userClaims.Concat(claims);
            }

            return userClaims.ToList();
        }
    }
}
