namespace Starter.Net.Api.Services
{
    public class LoginRequest
    {
        public string Login { set; get; }
        public string Password { set; get; }
        public string UserAgent { set; get; }
        public string IpAddress { set; get; }
        public string Location { set; get; }
    }
}
