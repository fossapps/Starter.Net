using Microsoft.AspNetCore.Mvc;

namespace Starter.Net.Api.Controllers
{
    public static class ControllerExtension
    {
        public static string GetIpAddress(this ControllerBase baseClass)
        {
            if (baseClass.Request.Headers.TryGetValue("X-Forwarded-For", out var ip))
            {
                return ip;
            }

            return baseClass.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public static string GetRoughLocation(this ControllerBase baseClass)
        {
            return "Bangkok"; // todo: find some way to get true rough location
        }
    }
}
