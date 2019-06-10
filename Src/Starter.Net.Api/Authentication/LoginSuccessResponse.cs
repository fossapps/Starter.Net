namespace Starter.Net.Api.Authentication
{
    public class LoginSuccessResponse
    {
        public string Jwt { set; get; }
        public string RefreshToken { set; get; }
    }
}
