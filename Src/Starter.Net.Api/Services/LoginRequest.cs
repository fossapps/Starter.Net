namespace Starter.Net.Api.Services
{
    public class LoginRequest: ViewModels.LoginRequest
    {
        public string UserAgent { set; get; }
        public string IpAddress { set; get; }
        public string Location { set; get; }
    }
}
