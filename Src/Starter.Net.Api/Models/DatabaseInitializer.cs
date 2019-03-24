using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Starter.Net.Api.Models
{
    public static class DatabaseInitializer
    {
        public static async Task SeedUsers(UserManager<User> userManager)
        {
            const string adminEmail = "admin@localhost";
            var user = await userManager.FindByEmailAsync(adminEmail);
            if (user != null)
            {
                return;
            }
            var result = await userManager.CreateAsync(new User()
            {
                Email = adminEmail,
                UserName = "admin"
            }, "password");
            Console.WriteLine(result.Errors.GetEnumerator().Current.Description);
        }
    }
}
