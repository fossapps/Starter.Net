using System.ComponentModel.DataAnnotations;

namespace Starter.Net.Api.Authentication
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string Login { set; get; }
    }
}
