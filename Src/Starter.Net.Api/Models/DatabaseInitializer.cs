using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Starter.Net.Api.Configs;

namespace Starter.Net.Api.Models
{
    public static class DatabaseInitializer
    {
        public static async Task SeedUsers(UserManager<User> userManager, IOptions<InitDb> initDb)
        {
            var users = initDb.Value.Users;
            if (!users.ShouldCreate)
            {
                return;
            }
            foreach (var usersDetail in users.Details)
            {
                var user = await userManager.FindByEmailAsync(usersDetail.Email);
                if (user != null)
                {
                    continue;
                }

                var dbUser = new User()
                {
                    Email = usersDetail.Email,
                    UserName = usersDetail.Username
                };
                var createUserResult = await userManager.CreateAsync(dbUser, usersDetail.Password);
                var addToRoleResult = await userManager.AddToRoleAsync(dbUser, usersDetail.Role);
                if (!createUserResult.Succeeded)
                {
                    Console.WriteLine(createUserResult.Errors.GetEnumerator().Current.Description);
                }
                if (!addToRoleResult.Succeeded)
                {
                    Console.WriteLine(createUserResult.Errors.GetEnumerator().Current.Description);
                }
            }
        }
    }
}
