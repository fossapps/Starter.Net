using System.ComponentModel.DataAnnotations;

namespace Starter.Net.Api.ViewModels
{
    public class UserRegistrationRequest
    {
        [Required]
        [MinLength(3)]
        public string Username { set; get; }

        [Required]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        public string Password { set; get; }
    }
}
