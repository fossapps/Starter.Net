using System.ComponentModel.DataAnnotations;

namespace Starter.Net.Api.ViewModels
{
    public class LoginRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
