using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starter.Net.Api.Models;
using Starter.Net.Api.ViewModels;

namespace Starter.Net.Api.Services
{
    public interface IUserService
    {
        Task<(SignInResult signInResult, LoginSuccessResponse login)> AuthenticateByUsername(LoginRequest request);
        Task<(SignInResult signInResult, LoginSuccessResponse login)> AuthenticateByEmail(LoginRequest request);
        Task<(SignInResult signInResult, LoginSuccessResponse login)> Authenticate(LoginRequest login);
        Task<RefreshTokenResponse> RefreshAuthentication(RefreshToken token);
        void RequestPasswordReset(User user);
        Task<IdentityResult> ResetPassword(PasswordResetRequest request);

        Task<Invitation> Invite(string emailTo, string fromUserId, string fromUserName);

        Task<(IdentityResult result, UserRegistrationSuccessResponse registrationSuccessResponse)> CreateUser(
            User user, string password);
    }
}
