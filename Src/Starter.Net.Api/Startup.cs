using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Starter.Net.Api.Configs;
using Starter.Net.Api.Models;
using Starter.Net.Api.Repositories;
using Starter.Net.Api.Services;

namespace Starter.Net.Api
{
    public class Startup : Starter.Net.Startup.StartupBase
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env): base(configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public new void ConfigureServices(IServiceCollection services)
        {
            AddConfiguration(services, Configuration);
            base.ConfigureServices(services);
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IRolesRepository, RolesRepository>();
            services.AddDbContext<ApplicationContext>();
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();
            services.AddMvc()
                .AddNewtonsoftJson();
            services.Configure<IdentityOptions>(ConfigureIdentityOptions);
        }

        private void ConfigureIdentityOptions(IdentityOptions options)
        {
            var authOptions = Configuration.GetSection("Authentication").Get<Configs.Authentication>();
            options.Password.RequireDigit = authOptions.PasswordRequirements.RequireDigit;
            options.Password.RequiredLength = authOptions.PasswordRequirements.RequiredLength;
            options.Password.RequireNonAlphanumeric = authOptions.PasswordRequirements.RequireAlphanumeric;
            options.Password.RequireUppercase = authOptions.PasswordRequirements.RequireUppercase;
            options.Password.RequireLowercase = authOptions.PasswordRequirements.RequireLowercase;

            options.Lockout.AllowedForNewUsers = authOptions.Lockouts.AllowedForNewUsers;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(authOptions.Lockouts.DefaultLockoutTimeInMinutes);
            options.Lockout.MaxFailedAccessAttempts = authOptions.Lockouts.MaxAllowedFailedAttempts;

            options.User.AllowedUserNameCharacters = authOptions.UsernameRequirements.AllowedCharactersInUsername;
            options.User.RequireUniqueEmail = authOptions.UsernameRequirements.RequireUniqueEmail;
        }
        private static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.Configure<InitDb>(configuration.GetSection("InitDb"));
            services.Configure<Configs.Authentication>(configuration.GetSection("Authentication"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting(routes => { routes.MapControllers(); });

            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
