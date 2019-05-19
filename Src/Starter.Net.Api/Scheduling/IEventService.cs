using System.Collections.Generic;
using Starter.Net.Api.Common;

namespace Starter.Net.Api.Scheduling
{
    public interface IEventService: ICrud<Event>
    {
        bool DoesEventConflicts(Event entry);
        IEnumerable<Event> FindEventsByCalendar(string calendar);
        IEnumerable<Event> FindEventsByCalendar(Calendar calendar);
    }
}
