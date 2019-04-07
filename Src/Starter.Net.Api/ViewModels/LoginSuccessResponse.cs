namespace Starter.Net.Api.ViewModels
{
    public class LoginSuccessResponse
    {
        public string Jwt { set; get; }
        public string RefreshToken { set; get; }
    }
}
