using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Starter.Net.Api.Authentication;
using Starter.Net.Api.Models;
using Starter.Net.Api.Repositories;
using Starter.Net.Api.Services;
using Starter.Net.Api.ViewModels;

namespace Starter.Net.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUserService _userService;

        public UsersController(IUsersRepository usersRepository, IUserService userService)
        {
            _usersRepository = usersRepository;
            _userService = userService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(UserRegistrationSuccessResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(UserRegistrationRequest userRegistrationRequest)
        {
            var user = new Models.User
            {
                Email = userRegistrationRequest.Email,
                UserName = userRegistrationRequest.Username
            };
            var (result, userRegistrationSuccessResponse) = await _userService.CreateUser(user, userRegistrationRequest.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Errors", error.Description);
                }
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            return CreatedAtAction("GetById", "Users", new { id = userRegistrationSuccessResponse.Id }, userRegistrationSuccessResponse);
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

        [HttpPost("invite")]
        [RequirePermission("beta.invite")]
        public async Task<IActionResult> Invite(InvitationRequest request)
        {
            var invitation = await _userService.Invite(request.Email, this.GetUserId(), this.GetUserName());
            return Ok(invitation);
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

        private IActionResult ProcessUserResult(Models.User user)
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
