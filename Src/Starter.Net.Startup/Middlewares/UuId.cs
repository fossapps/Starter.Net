using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Starter.Net.Startup.Services;

namespace Starter.Net.Startup.Middlewares
{
    public class UuId
    {
        private RequestDelegate _next { get; }
        private IUuidService _uuid { get; }
        public UuId(RequestDelegate next, IUuidService uuidService)
        {
            _next = next;
            _uuid = uuidService;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Cookies.TryGetValue("uuid", out var uuid))
            {
                uuid = _uuid.GenerateUuId();
                context.Response.Cookies.Append("uuid", uuid);
            }
            context.Items.Add("uuid", uuid);
            await _next(context);
        }
    }
}
