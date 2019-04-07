using System.ComponentModel.DataAnnotations;

namespace Starter.Net.Api.ViewModels
{
    public class PasswordResetRequest
    {
        [Required]
        public string Token { set; get; }

        [Required]
        public string Password { set; get; }
        
        [Required]
        public string Username { set; get; }
    }
}
