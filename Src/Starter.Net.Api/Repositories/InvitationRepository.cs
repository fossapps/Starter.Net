using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Starter.Net.Api.Models;
using Starter.Net.Startup.Services;

namespace Starter.Net.Api.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly ApplicationContext _db;
        private readonly IUuidService _uuidService;

        public InvitationRepository(ApplicationContext db, IUuidService uuidService)
        {
            _db = db;
            _uuidService = uuidService;
        }

        public async Task<Invitation> InviteUser(string fromUserId, string toEmailAddress)
        {
            var invitation = new Invitation()
            {
                Id = _uuidService.GenerateUuId(),
                To = toEmailAddress,
                NormalizedTo = toEmailAddress.ToUpper(),
                FromUserId = fromUserId
            };
            var entity = await _db.Invitations.AddAsync(invitation);
            await _db.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<bool> IsInvited(string emailAddress)
        {
            return await _db.Invitations.AnyAsync(x => x.NormalizedTo == emailAddress.ToUpper());
        }
    }
}
