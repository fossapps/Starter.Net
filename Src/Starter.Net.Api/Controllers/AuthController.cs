using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Starter.Net.Api.Models;
using Starter.Net.Api.Repositories;
using Starter.Net.Api.ViewModels;

namespace Starter.Net.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly UserManager<User> _userManager;

        public AuthController(UserManager<User> userManager, IUsersRepository usersRepository)
        {
            _userManager = userManager;
            _usersRepository = usersRepository;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(UserRegistrationSuccessResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(UserRegistrationRequest userRegistrationRequest)
        {
            var user = new User
            {
                Email = userRegistrationRequest.Email,
                UserName = userRegistrationRequest.Username
            };
            var result = await _userManager.CreateAsync(user, userRegistrationRequest.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Errors", error.Description);
                }
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var response = new UserRegistrationSuccessResponse()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName
            };
            return Created("/users/" + response.Id, response);
        }
    }
}
