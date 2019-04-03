namespace Starter.Net.Api.Configs
{
    public class Mail
    {
        public Smtp Smtp { set; get; }
    }

    public class Smtp
    {
        public string Host { set; get; }
        public int Port { set; get; }
        public string User { set; get; }
        public string Password { set; get; }
    }
}
