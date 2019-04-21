using System.ComponentModel.DataAnnotations;

namespace Starter.Net.Api.ViewModels
{
    public class InvitationRequest
    {
        [Required]
        [EmailAddress]
        public string Email { set; get; }
    }
}
