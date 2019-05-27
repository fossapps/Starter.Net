using System.Collections.Generic;
using Starter.Net.Api.Scheduling;

namespace Starter.Net.Api.Users
{
    public interface IUserCalendarService
    {
        void LinkCalendarToUser(string calendar, string user);
        IEnumerable<Calendar> FindCalendarForUser(string user);
    }
}
