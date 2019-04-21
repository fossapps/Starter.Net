namespace Starter.Net.Api.Models
{
    public class Invitation
    {
        public string Id { set; get; }
        public string FromUserId { set; get; }
        public string To { set; get; }
        public string NormalizedTo { set; get; }
    }
}
