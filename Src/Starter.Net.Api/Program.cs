using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Starter.Net.Api.Configs;
using Starter.Net.Api.Models;

namespace Starter.Net.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();
            using (var scope = webHost.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var userManager = serviceProvider.GetRequiredService<UserManager<Models.User>>();
                    var options = serviceProvider.GetRequiredService<IOptions<InitDb>>();
                    DatabaseInitializer.SeedUsers(userManager, options).Wait();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            webHost.Run();
        }

        private static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
