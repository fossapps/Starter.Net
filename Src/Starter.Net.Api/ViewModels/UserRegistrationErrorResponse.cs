using System.Collections.Generic;

namespace Starter.Net.Api.ViewModels
{
    public class UserRegistrationErrorResponse
    {
        public IEnumerable<string> Errors { set; get; }
    }
}
