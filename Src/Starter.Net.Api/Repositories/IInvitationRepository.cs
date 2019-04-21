using System.Threading.Tasks;
using Starter.Net.Api.Models;

namespace Starter.Net.Api.Repositories
{
    public interface IInvitationRepository
    {
        Task<Invitation> InviteUser(string fromUserId, string toEmailAddress);
        Task<bool> IsInvited(string emailAddress);
    }
}
