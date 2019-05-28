using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using reCAPTCHA.AspNetCore;
using Starter.Net.Api.Configs;
using Starter.Net.Api.Mails;
using Starter.Net.Api.Models;
using Starter.Net.Api.Repositories;
using Starter.Net.Api.Scheduling;
using Starter.Net.Api.Services;
using Starter.Net.Api.Users;

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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            AddConfiguration(services, Configuration);
            base.ConfigureServices(services);
            services.AddSingleton<IMailService, SmtpMailService>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ICalendarRepository, CalendarRepository>();
            services.AddScoped<IUserCalendarService, UserCalendarService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole>>();
            services.AddScoped<DbContext, ApplicationContext>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddSingleton<IRecaptchaService, RecaptchaService>();
            services.AddSingleton<ITokenFactory, TokenFactory>();
            services.AddDbContext<ApplicationContext>();
            services.AddIdentity<User, IdentityRole>(options =>
                    {
                        options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
                    })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthorization();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    var authOptions = Configuration.GetSection("Authentication").Get<Configs.Authentication>();
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = false;
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = authOptions.JwtBearerOptions.Issuer,
                        ValidAudience = authOptions.JwtBearerOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.JwtBearerOptions.SigningKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddMvc()
                .AddNewtonsoftJson();
            services.Configure<IdentityOptions>(ConfigureIdentityOptions);
            services.Configure<KestrelServerOptions>(x => { x.AllowSynchronousIO = true;});
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

            options.SignIn.RequireConfirmedEmail = false; // todo: move to config
        }
        private static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.Configure<Database>(configuration.GetSection("DatabaseConfig"));
            services.Configure<RecaptchaSettings>(configuration.GetSection("RecaptchaSettings"));
            services.Configure<InitDb>(configuration.GetSection("InitDb"));
            services.Configure<Mail>(configuration.GetSection("EmailConfig"));
            services.Configure<Configs.Authentication>(configuration.GetSection("Authentication"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");
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

//            app.UseHttpsRedirection();

            app.UseRouting(routes => { routes.MapControllers(); });

            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
