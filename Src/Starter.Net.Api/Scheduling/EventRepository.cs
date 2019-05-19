using System;
using System.Collections.Generic;
using System.Linq;
using Starter.Net.Api.Models;
using Starter.Net.Startup.Services;

namespace Starter.Net.Api.Scheduling
{
    public class EventRepository: IEventRepository
    {
        private readonly ApplicationContext _db;
        private readonly IUuidService _uuidService;

        public EventRepository(IUuidService uuidService, ApplicationContext db)
        {
            _uuidService = uuidService;
            _db = db;
        }

        public Event Create(Event item)
        {
            if (string.IsNullOrWhiteSpace(item.Id))
            {
                item.Id = _uuidService.GenerateUuId();
            }

            _db.Events.Add(item);
            _db.SaveChangesAsync();
            return item;
        }

        public void Update(string id, Event item)
        {
            var e = new Event {Id = id};
            _db.Events.Attach(e);
            e.Description = item.Description;
            e.End = item.End;
            e.Location = item.Location;
            e.Start = item.Start;
            e.Title = item.Title;
            e.Calendar = item.Calendar;
            _db.SaveChangesAsync();
        }

        public void Remove(string id)
        {
            var e = new Event {Id = id};
            _db.Events.Attach(e);
            _db.Events.Remove(e);
            _db.SaveChangesAsync();
        }

        public Event Find(string id)
        {
            return _db.Events.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Event> FindEventsForCalendar(string calendar)
        {
            return _db.Events.Where(x => x.Calendar == calendar);
        }

        public bool IsTimeSlotAvailable(DateTime start, DateTime end)
        {
            return _db.Events.Count(x => start < x.End && end > x.Start) == 0;
        }
    }
}
