using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Starter.Net.Api.Repositories;

namespace Starter.Net.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpHead("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UsernameAvailability(string username)
        {
            var user = await _usersRepository.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpHead("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EmailAvailability(string email)
        {
            var user = await _usersRepository.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
