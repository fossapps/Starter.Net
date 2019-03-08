using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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
    }
}
