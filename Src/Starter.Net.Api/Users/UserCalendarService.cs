using System.Collections.Generic;
using System.Linq;
using Starter.Net.Api.Models;
using Starter.Net.Api.Scheduling;
using Starter.Net.Startup.Services;

namespace Starter.Net.Api.Users
{
    public class UserCalendarService: IUserCalendarService
    {
        private readonly ApplicationContext _db;
        private readonly IUuidService _uuidService;

        public UserCalendarService(IUuidService uuidService, ApplicationContext db)
        {
            _uuidService = uuidService;
            _db = db;
        }

        public void LinkCalendarToUser(string calendar, string user)
        {
            var entry = new UserToCalendar()
            {
                Calendar = calendar,
                User = user,
                Id = _uuidService.GenerateUuId()
            };
            _db.UserToCalendars.Add(entry);
            _db.SaveChanges();
        }

        public IEnumerable<Calendar> FindCalendarForUser(string user)
        {
            var q = from uc in _db.UserToCalendars
                join c in _db.Calendars on uc.Calendar equals c.Id
                where uc.User == user
                select new Calendar { Id = c.Id, Name = c.Name, CreatedAt = c.CreatedAt };
            return q.AsEnumerable();
        }
    }
}
