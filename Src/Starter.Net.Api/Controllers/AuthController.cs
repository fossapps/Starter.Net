using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Starter.Net.Api.Authentication;
using Starter.Net.Api.Configs;
using Starter.Net.Api.Mails;
using Starter.Net.Api.Mails.Content;
using Starter.Net.Api.Models;
using Starter.Net.Api.Repositories;
using Starter.Net.Api.Services;
using Starter.Net.Api.ViewModels;
using Starter.Net.Startup.Services;

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
        private readonly IUsersRepository _usersRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly IMailService _mailService;
        private readonly Mail _mailConfig;

        public AuthController(
            UserManager<User> userManager,
            IUserService userService,
            IUuidService uuidService,
            ITokenFactory tokenFactory,
            ApplicationContext db,
            IOptions<Configs.Authentication> authentication,
            IUsersRepository usersRepository,
            SignInManager<User> signInManager,
            IMailService mailService,
            IOptions<Mail> mailConfig
            )
        {
            _userManager = userManager;
            _userService = userService;
            _uuidService = uuidService;
            _tokenFactory = tokenFactory;
            _db = db;
            _usersRepository = usersRepository;
            _signInManager = signInManager;
            _mailService = mailService;
            _mailConfig = mailConfig.Value;
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

            var jwt = _tokenFactory.GenerateJwtToken(principal);
            var refreshToken = GetRefreshToken(user.Id);
            _db.RefreshTokens.Add(refreshToken);
            _db.SaveChanges();
            var loginResponse = new LoginSuccessResponse()
            {
                RefreshToken = refreshToken.Value,
                Jwt = jwt
            };
            return Ok(loginResponse);
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
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var mailBuilder = new ForgotPasswordEmail(_mailConfig);
            var mail = mailBuilder.Build("/localhost/api/auth/" + token, new MailAddress(user.Email, user.UserName));
            _mailService.Send(mail);
            return Ok();
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Refresh([FromHeader] string authorization)
        {
            var token = _db.RefreshTokens.SingleOrDefault(x => x.Value == GetBearerToken(authorization));
            if (token == null)
            {
                return BadRequest();
            }
            var user = await _usersRepository.FindByUserIdAsync(token.User);
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var res = new RefreshTokenResponse()
            {
                Token = _tokenFactory.GenerateJwtToken(principal)
            };
            return Ok(res);
        }

        private string GetBearerToken(string authorizationHeader)
        {
            return authorizationHeader.Substring("Bearer ".Length).Trim();
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword(PasswordResetRequest request)
        {
            var user = await _usersRepository.FindByNameAsync(request.Username);
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Errors", error.Description);
                }
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var builder = new PasswordResetConfirmationEmail(_mailConfig);
            var recipient = new MailAddress(user.Email, user.UserName);
            var mail = builder.Build(recipient);
            _mailService.Send(mail);
            return Ok();
        }

        [HttpGet("check")]
        [RequirePermission("sudo")]
        public async Task<string> Check()
        {
            return "ok";
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
