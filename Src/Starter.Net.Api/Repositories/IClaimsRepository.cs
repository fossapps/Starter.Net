using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starter.Net.Api.Models;

namespace Starter.Net.Api.Repositories
{
    public interface IClaimsRepository
    {
        Task<IList<Claim>> GetClaimsForRole(string roleName);
        Task<IList<Claim>> GetClaimsForUserId(string userId);
        Task<IList<Claim>> GetClaimsForUser(User user);
    }
}
