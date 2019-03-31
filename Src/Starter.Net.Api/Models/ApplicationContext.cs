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
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
