using System.ComponentModel.DataAnnotations;

namespace Starter.Net.Api.Scheduling
{
    public class CreateCalendarRequest
    {
        [Required]
        public string Name { set; get; }
    }
}
