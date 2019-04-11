using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Starter.Net.Api.Authentication;
using Starter.Net.Api.Models;
using Starter.Net.Api.Repositories;
using Starter.Net.Api.Services;
using Starter.Net.Api.ViewModels;
using LoginRequest = Starter.Net.Api.ViewModels.LoginRequest;

namespace Starter.Net.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUsersRepository _usersRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public SessionController(
            IUserService userService,
            IUsersRepository usersRepository,
            IRefreshTokenRepository refreshTokenRepository
            )
        {
            _userService = userService;
            _usersRepository = usersRepository;
            _refreshTokenRepository = refreshTokenRepository;
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

        [HttpPost("new")]
        [ProducesResponseType(typeof(LoginSuccessResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginRequest loginRequest, [FromHeader(Name = "User-Agent")] string useragent)
        {
            var request = new Services.LoginRequest()
            {
                Login = loginRequest.Login,
                Password = loginRequest.Password,
                Location = this.GetRoughLocation(),
                IpAddress = this.GetIpAddress(),
                UserAgent = useragent
            };
            var (signInResult, res) = await _userService.Authenticate(request);
            if (!signInResult.Succeeded)
            {
                return Unauthorized(ProcessErrorResult(signInResult));
            }
            return Ok(res);
        }

        [HttpPost("request_reset")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Reset(ForgotPasswordRequest request)
        {
            // once rate limiting is done, we can limit by two keys, one with request.Login and another with client's IP
            // should we give error message if user doesn't exist? we can do this by config
            // in either case, we should add rate limiting.
            var user = await _usersRepository.Find(request.Login);
            if (user == null)
            {
                return NotFound();
            }
            _userService.RequestPasswordReset(user);
            return Ok();
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Refresh([FromHeader] string authorization)
        {
            var token = _refreshTokenRepository.FindByToken(GetBearerToken(authorization));
            if (token == null)
            {
                return BadRequest();
            }
            return Ok(await _userService.RefreshAuthentication(token));
        }

        private static string GetBearerToken(string authorizationHeader)
        {
            return authorizationHeader.Substring("Bearer ".Length).Trim();
        }

        [HttpGet("list")]
        [Authorize]
        public IActionResult List()
        {
            // UserClaims.UserId, get all refresh tokens, return them
            var subject = User.Claims.FirstOrDefault(c => c.Type == "sub");
            if (subject == null)
            {
                throw new Exception("subject not found");
            }
            var tokens = _refreshTokenRepository
                .FindForUser(subject.Value)
                .Select(x => new RefreshTokenResponse() {Token = x.Value});
            return Ok(tokens);
        }

        [HttpDelete("{refresh_token}")]
        [Authorize]
        public IActionResult Delete(string refreshToken)
        {
            // first check if the refresh token belongs to current user
            var subject = User.Claims.FirstOrDefault(c => c.Type == "sub");
            if (subject == null)
            {
                throw new Exception("subject not found");
            }

            var token = _refreshTokenRepository.FindByToken(refreshToken);
            if (token.User != subject.Value)
            {
                throw new Exception("Token doesn't belong to this user");
            }
            _refreshTokenRepository.DeleteByToken(token.Value);
            return Ok();
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword(PasswordResetRequest request)
        {
            var result = await _userService.ResetPassword(request);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Errors", error.Description);
                }
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            return Ok();
        }

        [HttpGet("check")]
        [RequirePermission("sudo")]
        public IActionResult Check()
        {
            return Ok("ok");
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
