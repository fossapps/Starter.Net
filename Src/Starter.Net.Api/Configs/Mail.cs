namespace Starter.Net.Api.Configs
{
    public class Mail
    {
        public Smtp Smtp { set; get; }
        public Sender DefaultSender { set; get; }
    }

    public class Sender
    {
        public string From { set; get; }
        public string Name { set; get; }
    }
    public class Smtp
    {
        public string Host { set; get; }
        public int Port { set; get; }
        public string User { set; get; }
        public string Password { set; get; }
    }
}
