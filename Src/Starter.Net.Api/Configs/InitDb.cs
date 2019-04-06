namespace Starter.Net.Api.Configs
{
    public class Users
    {
        public bool ShouldCreate { set; get; }
        public Details[] Details { set; get; }
    }
    public class Details
    {
        public string Email { set; get; }
        public string Username { set; get; }
        public string Role { set; get; }
        public string Password { set; get; }
    }

    public class InitDb
    {
        public Users Users { set; get; }
    }
}
