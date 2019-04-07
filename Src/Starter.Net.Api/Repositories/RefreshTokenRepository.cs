using System.Collections.Generic;
using System.Linq;
using Starter.Net.Api.Models;

namespace Starter.Net.Api.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationContext _db;
        public RefreshTokenRepository(ApplicationContext context)
        {
            _db = context;
        }

        public void Add(RefreshToken token)
        {
            _db.RefreshTokens.Add(token);
            _db.SaveChangesAsync();
        }

        public RefreshToken FindByToken(string token)
        {
            return _db.RefreshTokens.SingleOrDefault(x => x.Value == token);
        }

        public IEnumerable<RefreshToken> FindForUser(string userId)
        {
            return _db.RefreshTokens.Where(x => x.User == userId);
        }
    }
}
