using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Starter.Net.Api.Models;
using Starter.Net.Api.Services;
using Starter.Net.Api.ViewModels;
using Starter.Net.Startup.Middlewares;
using Starter.Net.Startup.Services;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Starter.Net.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IUuidService _uuidService;
        private readonly ITokenFactory _tokenFactory;
        private readonly ApplicationContext _db;

        public AuthController(UserManager<User> userManager, IUserService userService, IUuidService uuidService, ITokenFactory tokenFactory, ApplicationContext db)
        {
            _userManager = userManager;
            _userService = userService;
            _uuidService = uuidService;
            _tokenFactory = tokenFactory;
            _db = db;
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

        [HttpPost("authorize")]
        [ProducesResponseType(typeof(LoginSuccessResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var (signInResult, principal, user) = await _userService.Authenticate(loginRequest.Login, loginRequest.Password);
            if (!signInResult.Succeeded)
            {
                return Unauthorized(ProcessErrorResult(signInResult));
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("qwertyuiopasdfghjklzxcvbnm123456");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(principal.Claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GetRefreshToken(user.Id);
            _db.RefreshTokens.Add(refreshToken);
            var loginResponse = new LoginSuccessResponse()
            {
                RefreshToken = refreshToken.Value,
                Jwt = tokenHandler.WriteToken(jwt)
            };
            return Ok(loginResponse);
        }

        private RefreshToken GetRefreshToken(string userId)
        {
            return new RefreshToken()
            {
                Id = _uuidService.GenerateUuId(),
                User = userId,
                Value = _tokenFactory.GenerateToken(32)
            };
        }

        private static string ProcessErrorResult(Microsoft.AspNetCore.Identity.SignInResult result)
        {
            if (result.IsLockedOut)
            {
                return "Account Locked Out";
            }

            return result.IsNotAllowed ? "Login not allowed" : "Wrong Credentials";
        }
    }
}
