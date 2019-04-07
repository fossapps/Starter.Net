using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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

        public UserService(
            SignInManager<User> signInManager,
            IUsersRepository usersRepository,
            ITokenFactory tokenFactory,
            IUuidService uuidService,
            IRefreshTokenRepository refreshTokenRepository
            )
        {
            _signInManager = signInManager;
            _usersRepository = usersRepository;
            _tokenFactory = tokenFactory;
            _uuidService = uuidService;
            _refreshTokenRepository = refreshTokenRepository;
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
