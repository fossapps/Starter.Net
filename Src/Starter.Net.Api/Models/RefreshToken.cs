namespace Starter.Net.Api.Models
{
    public class RefreshToken
    {
        public string Id { set; get; }
        public string User { set; get; }
        public string Value { set; get; }
        public string Useragent { set; get; }
        public string IpAddress { set; get; }
        public string Location { set; get; }
    }
}
