using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Starter.Net.Api.Authentication;
using Starter.Net.Api.Configs;
using Starter.Net.Api.Mails;
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
        private readonly JwtBearerOptions _jwt;
        private readonly IUsersRepository _usersRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly IMailService _mailService;

        public AuthController(
            UserManager<User> userManager,
            IUserService userService,
            IUuidService uuidService,
            ITokenFactory tokenFactory,
            ApplicationContext db,
            IOptions<Configs.Authentication> authentication,
            IUsersRepository usersRepository,
            SignInManager<User> signInManager,
            IMailService mailService
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
            _jwt = authentication.Value.JwtBearerOptions;
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
            var key = Encoding.ASCII.GetBytes(_jwt.SigningKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwt.Issuer,
                Audience = _jwt.Audience,
                Subject = new ClaimsIdentity(principal.Claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwt.JwtTtl),
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
            var mailBuilder = new MailMessageBuilder();
            var recipient = new MailAddress(user.Email, user.UserName);
            var mail = mailBuilder.WithSubject("Reset Password")
                .WithSender(new MailAddress("no-reply@starter.net", "Starter.Net"))
                .From(new MailAddress("no-reply@starter.net", "Starter.Net"))
                .WithPlainTextBody(token)
                .AddRecipients(new MailAddressCollection {recipient})
                .Build();
            _mailService.Send(mail);
            return Ok();
            // create a jwt and send email
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

            var builder = new MailMessageBuilder();
            var recipient = new MailAddress(user.Email, user.UserName);
            var mail = builder.From(new MailAddress("no-reply@starter.net", "Starter.Net"))
                .WithSubject("Password Reset Successful")
                .WithPlainTextBody("Password Reset")
                .AddRecipients(new MailAddressCollection() {recipient})
                .Build();
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
