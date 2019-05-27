using System.Collections.Generic;
using System.IO;

namespace Starter.Net.Api.Scheduling
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Event Create(Event item)
        {
            if (!_eventRepository.IsTimeSlotAvailable(item.Start, item.End))
            {
                throw new InvalidDataException("event conflicts with existing event");
            }
            // check first
            return _eventRepository.Create(item);
        }

        public void Update(string id, Event item)
        {
            _eventRepository.Update(id, item);
        }

        public void Remove(string id)
        {
            _eventRepository.Remove(id);
        }

        public Event Find(string id)
        {
            return _eventRepository.Find(id);
        }

        public bool DoesEventConflicts(Event entry)
        {
            return !_eventRepository.IsTimeSlotAvailable(entry.Start, entry.End);
        }

        public IEnumerable<Event> FindEventsByCalendar(string calendar)
        {
            return _eventRepository.FindEventsForCalendar(calendar);
        }

        public IEnumerable<Event> FindEventsByCalendar(Calendar calendar)
        {
            return FindEventsByCalendar(calendar.Id);
        }
    }
}
