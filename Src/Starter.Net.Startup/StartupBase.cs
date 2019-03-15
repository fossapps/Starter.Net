using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Starter.Net.Startup.Middlewares;
using Starter.Net.Startup.Services;

namespace Starter.Net.Startup
{
    public abstract class StartupBase
    {
        private IConfiguration Config { get; }

        protected StartupBase(IConfiguration config)
        {
            Config = config;
        }

        protected void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUuidService>(new UuIdService());
        }

        protected void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<UuId>();
        }
    }
}
