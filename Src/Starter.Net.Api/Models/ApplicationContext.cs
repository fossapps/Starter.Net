using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Starter.Net.Api.Authentication;

namespace Starter.Net.Api.Models
{
    public class ApplicationContext: IdentityDbContext
    {
        public DbSet<User> Users { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = new NpgsqlConnectionStringBuilder	
            {
                Host = "localhost",	
                Port = 15432,	
                Database = "starter",	
                Username = "fossapps",	
                Password = "secret",	
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
