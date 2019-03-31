using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Starter.Net.Api.Authentication;
using Starter.Net.Api.Configs;

namespace Starter.Net.Api.Models
{
    public class ApplicationContext: IdentityDbContext
    {
        public DbSet<User> Users { set; get; }
        private readonly Database _db;

        public ApplicationContext(DbContextOptions options, IOptions<Database> dbOption) : base(options)
        {
            _db = dbOption.Value;
        }

        protected ApplicationContext(IOptions<Database> dbOption)
        {
            _db = dbOption.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = new NpgsqlConnectionStringBuilder	
            {
                Host = _db.Host,
                Port = _db.Port,
                Database = _db.Name,
                Username = _db.User,
                Password = _db.Password,
                SslMode = SslMode.Disable
            };
            optionsBuilder.UseNpgsql(connection.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            IdentityRole[] roles =
            {
                new IdentityRole("admin") {Id = "9a6eb015-82d1-480c-b962-5aab596ef4f6"}
            };
            builder.Entity<IdentityRole>().HasData(roles);
            IdentityRoleClaim<string>[] claims =
            {
                new IdentityRoleClaim<string>()
                {
                    Id = 1,
                    RoleId = roles[0].Id,
                    ClaimType = CustomClaims.Permission,
                    ClaimValue = "sudo",
                },
            };
            builder.Entity<IdentityRoleClaim<string>>().HasData(claims);
        }
    }
}
