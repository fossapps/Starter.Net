using System;
using System.ComponentModel.DataAnnotations;

namespace Starter.Net.Api.Scheduling
{
    public class CalendarEvent
    {
        [Required]
        public string Title { set; get; }

        [Required]
        public string Description { set; get; }
        public string Location { set; get; }

        [Required]
        public DateTime Start { set; get; }

        [Required]
        public DateTime End { set; get; }
    }
}
