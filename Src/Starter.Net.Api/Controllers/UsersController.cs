using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Starter.Net.Api.Models;
using Starter.Net.Api.Repositories;
using Starter.Net.Api.ViewModels;

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

        [HttpGet("name/{username}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserName(string username)
        {
            return ProcessUserResult(await _usersRepository.FindByNameAsync(username));
        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            return ProcessUserResult(await _usersRepository.FindByEmailAsync(email));
        }

        [HttpGet("id/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(string id)
        {
            return ProcessUserResult(await _usersRepository.FindByUserIdAsync(id));
        }

        private IActionResult ProcessUserResult(User user)
        {
            if (user == null)
            {
                return NotFound();
            }

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName
            };
            return Ok(userResponse);
        }
    }
}
