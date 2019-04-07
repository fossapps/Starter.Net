using System.ComponentModel.DataAnnotations;

namespace Starter.Net.Api.ViewModels
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string Login { set; get; }
    }
}
