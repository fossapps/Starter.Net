using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Starter.Net.Api.Scheduling;
using Starter.Net.Api.User;
using Starter.Net.Api.ViewModels;
using Calendar = Starter.Net.Api.Scheduling.Calendar;

namespace Starter.Net.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulingController: ControllerBase
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly IUserCalendarService _userCalendar;
        private readonly IEventService _eventService;

        public SchedulingController(ICalendarRepository calendarRepository, IUserCalendarService userCalendar, IEventService eventService)
        {
            _calendarRepository = calendarRepository;
            _userCalendar = userCalendar;
            _eventService = eventService;
        }

        [Authorize]
        [HttpGet("calendars")]
        public async Task<IActionResult> GetAllCalendars()
        {
            var calendars = _userCalendar.FindCalendarForUser(this.GetUserId()).Select(x => new ViewModels.Calendar()
            {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt
            });
            return Ok(calendars);
        }

        [Authorize]
        [HttpPost("calendars")]
        public async Task<IActionResult> CreateCalendar(CreateCalendarRequest request)
        {
            var calendar = _calendarRepository.Create(new Calendar()
            {
                Name = request.Name
            });
            _userCalendar.LinkCalendarToUser(calendar.Id, this.GetUserId());
            return Ok();
        }

        [Authorize]
        [HttpGet("calendar/{id}")]
        public async Task<IActionResult> GetCalendarEvents([FromRoute] string id)
        {
            var events = _eventService.FindEventsByCalendar(id);
            return Ok(events);
        }

        [Authorize]
        [HttpPost("calendar/{id}")]
        public async Task<IActionResult> CreateCalendarEvent([FromRoute] string id, CalendarEvent ev)
        {
            var calendarEvent = new Event()
            {
                Calendar = id,
                Description = ev.Description,
                End = ev.End,
                Location = ev.Location,
                Start = ev.Start,
                Title = ev.Title
            };
            if (_eventService.DoesEventConflicts(calendarEvent))
            {
                return BadRequest();
            }

            return Ok(_eventService.Create(calendarEvent));
        }
    }
}
