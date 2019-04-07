using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Starter.Net.Api.Configs;
using Starter.Net.Api.Mails;
using Starter.Net.Api.Mails.Content;
using Starter.Net.Api.Models;
using Starter.Net.Api.Repositories;
using Starter.Net.Api.ViewModels;
using Starter.Net.Startup.Services;

namespace Starter.Net.Api.Services
{
    public class UserService: IUserService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenFactory _tokenFactory;
        private readonly IUuidService _uuidService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly Mail _mailConfig;
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;

        public UserService(
            SignInManager<User> signInManager,
            IUsersRepository usersRepository,
            ITokenFactory tokenFactory,
            IUuidService uuidService,
            IRefreshTokenRepository refreshTokenRepository,
            IOptions<Mail> mailConfig,
            UserManager<User> userManager,
            IMailService mailService
            )
        {
            _signInManager = signInManager;
            _usersRepository = usersRepository;
            _tokenFactory = tokenFactory;
            _uuidService = uuidService;
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
            _mailService = mailService;
            _mailConfig = mailConfig.Value;
        }

        public async Task<RefreshTokenResponse> RefreshAuthentication(RefreshToken token)
        {
            var user = await _usersRepository.FindByUserIdAsync(token.User);
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            return new RefreshTokenResponse()
            {
                Token = _tokenFactory.GenerateJwtToken(principal)
            };
        }

        public async void RequestPasswordReset(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var mailBuilder = new ForgotPasswordEmail(_mailConfig);
            var mail = mailBuilder.Build("/localhost/api/auth/" + token, new MailAddress(user.Email, user.UserName));
            _mailService.Send(mail);
        }

        public async Task<IdentityResult> ResetPassword(PasswordResetRequest request)
        {
            var user = await _usersRepository.FindByNameAsync(request.Username);
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!result.Succeeded)
            {
                return result;
            }
            var builder = new PasswordResetConfirmationEmail(_mailConfig);
            var recipient = new MailAddress(user.Email, user.UserName);
            var mail = builder.Build(recipient);
            _mailService.Send(mail);
            return result;
        }

        public async Task<(IdentityResult result, UserRegistrationSuccessResponse registrationSuccessResponse)> CreateUser(User user, string password)
        {
            var (result, registrationSuccessResponse, token) = await _usersRepository.Create(user, password);
            if (!result.Succeeded)
            {
                return (result, registrationSuccessResponse);
            }

            user.NormalizedEmail = user.Email.ToUpper();
            user.NormalizedUserName = user.UserName.ToUpper();
            var mail = new ActivationEmail(_mailConfig);
            _mailService.Send(mail.Build("/activate/" + token, new MailAddress(user.Email, user.UserName)));
            return (result, registrationSuccessResponse);
        }

        public Task<(SignInResult signInResult, LoginSuccessResponse login)> Authenticate(string login, string password)
        {
            return login.Contains("@")
                ? AuthenticateByEmail(login, password)
                : AuthenticateByUsername(login, password);
        }

        public async Task<(SignInResult signInResult, LoginSuccessResponse login)> AuthenticateByUsername(string username, string password)
        {
            var user = await _usersRepository.FindByNameAsync(username);
            return await AuthenticateUser(user, password);
        }

        public async Task<(SignInResult signInResult, LoginSuccessResponse login)> AuthenticateByEmail(string email, string password)
        {
            var user = await _usersRepository.FindByEmailAsync(email);
            return await AuthenticateUser(user, password);
        }

        private async Task<(SignInResult signInResult, LoginSuccessResponse login)> AuthenticateUser(User user, string password)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, true);
            if (!signInResult.Succeeded)
            {
                return (signInResult, null);
            }
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var jwt = _tokenFactory.GenerateJwtToken(principal);
            var refreshToken = new RefreshToken()
            {
                Id = _uuidService.GenerateUuId(),
                User = user.Id,
                Value = _tokenFactory.GenerateToken(32)
            };
            _refreshTokenRepository.Add(refreshToken);
            var res = new LoginSuccessResponse()
            {
                RefreshToken = refreshToken.Value,
                Jwt = jwt
            };
            return (signInResult, res);
        }
    }
}
