using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Starter.Net.Startup.Middlewares;
using Starter.Net.Startup.Services;

namespace Starter.Net.Startup
{
    public abstract class StartupBase
    {
        private IHostingEnvironment _env { get; set; }
        private IConfiguration _config { set; get; }

        protected StartupBase(IHostingEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
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
