using System;

namespace Starter.Net.Api.Scheduling
{
    public class Event
    {
        public string Id { set; get; }
        public string Calendar { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public string Location { set; get; }
        public DateTime Start { set; get; }
        public DateTime End { set; get; }
    }
}
