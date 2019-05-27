using System;
using System.Collections.Generic;
using Starter.Net.Api.Common;

namespace Starter.Net.Api.Scheduling
{
    public interface IEventRepository: ICrud<Event>
    {
        IEnumerable<Event> FindEventsForCalendar(string calendar);

        bool IsTimeSlotAvailable(DateTime start, DateTime end);
    }
}
