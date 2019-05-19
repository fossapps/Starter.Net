using System.Collections.Generic;
using Starter.Net.Api.Common;

namespace Starter.Net.Api.Scheduling
{
    public interface ICalendarRepository: ICrud<Calendar>
    {
        IEnumerable<Event> GetEvents(Calendar calendar);
        void AddEvent(Calendar calendar, Event item);
        IEnumerable<Calendar> GetAll();
    }
}
