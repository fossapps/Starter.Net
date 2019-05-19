using System;

namespace Starter.Net.Api.ViewModels
{
    public class CalendarEvent
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public string Location { set; get; }
        public DateTime Start { set; get; }
        public DateTime End { set; get; }
    }
}
