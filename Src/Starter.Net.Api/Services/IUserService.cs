using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starter.Net.Api.Models;
using Starter.Net.Api.ViewModels;

namespace Starter.Net.Api.Services
{
    public interface IUserService
    {
        Task<(SignInResult signInResult, LoginSuccessResponse login)> AuthenticateByUsername(string username, string password);
        Task<(SignInResult signInResult, LoginSuccessResponse login)> AuthenticateByEmail(string email, string password);
        Task<(SignInResult signInResult, LoginSuccessResponse login)> Authenticate(string login, string password);
        Task<RefreshTokenResponse> RefreshAuthentication(RefreshToken token);
        void RequestPasswordReset(User user);
        Task<IdentityResult> ResetPassword(PasswordResetRequest request);

        Task<(IdentityResult result, UserRegistrationSuccessResponse registrationSuccessResponse)> CreateUser(
            User user, string password);
    }
}
