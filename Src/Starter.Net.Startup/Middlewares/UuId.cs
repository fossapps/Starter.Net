using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Starter.Net.Startup.Services;

namespace Starter.Net.Startup.Middlewares
{
    public class UuId
    {
        private RequestDelegate Next { get; }
        private IUuidService Uuid { get; }
        public UuId(RequestDelegate next, IUuidService uuidService)
        {
            Next = next;
            Uuid = uuidService;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Cookies.TryGetValue("uuid", out var uuid))
            {
                uuid = Uuid.GenerateUuId();
                context.Response.Cookies.Append("uuid", uuid);
            }
            context.Items.Add("uuid", uuid);
            await Next(context);
        }
    }
}
