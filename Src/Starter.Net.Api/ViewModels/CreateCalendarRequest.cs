using System.ComponentModel.DataAnnotations;

namespace Starter.Net.Api.ViewModels
{
    public class CreateCalendarRequest
    {
        [Required]
        public string Name { set; get; }
    }
}
