using System;
using System.Collections.Generic;
using System.Linq;
using Starter.Net.Api.Models;
using Starter.Net.Startup.Services;

namespace Starter.Net.Api.Scheduling
{
    public class CalendarRepository: ICalendarRepository
    {
        private readonly ApplicationContext _db;
        private readonly IUuidService _uuidService;
        private readonly IEventService _eventService;

        public CalendarRepository(ApplicationContext db, IUuidService uuidService, IEventService eventService)
        {
            _db = db;
            _uuidService = uuidService;
            _eventService = eventService;
        }

        public Calendar Create(Calendar item)
        {
            if (string.IsNullOrWhiteSpace(item.Id))
            {
                item.Id = _uuidService.GenerateUuId();
            }
            item.CreatedAt = DateTime.Now;

            _db.Calendars.Add(item);
            _db.SaveChanges();
            return item;
        }

        public void Update(string id, Calendar item)
        {
            var c = new Calendar() { Id = id };
            _db.Calendars.Attach(c);
            c.Name = item.Name;
            _db.SaveChangesAsync();
        }

        public void Remove(string id)
        {
            var c = new Calendar() {Id = id};
            _db.Calendars.Attach(c);
            _db.Calendars.Remove(c);
            _db.SaveChangesAsync();
        }

        public Calendar Find(string id)
        {
            return _db.Calendars.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Event> GetEvents(Calendar calendar)
        {
            return _eventService.FindEventsByCalendar(calendar.Id);
        }

        public void AddEvent(Calendar calendar, Event item)
        {
            _eventService.Create(item);
        }

        public IEnumerable<Calendar> GetAll()
        {
            return _db.Calendars.Where(x => true);
        }
    }
}
