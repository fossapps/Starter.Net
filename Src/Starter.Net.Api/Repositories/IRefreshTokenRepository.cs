using System.Collections.Generic;
using Starter.Net.Api.Models;

namespace Starter.Net.Api.Repositories
{
    public interface IRefreshTokenRepository
    {
        void Add(RefreshToken token);
        RefreshToken FindByToken(string token);
        IEnumerable<RefreshToken> FindForUser(string userId);
        void DeleteByToken(string refreshToken);
        void DeleteById(string id);
    }
}
