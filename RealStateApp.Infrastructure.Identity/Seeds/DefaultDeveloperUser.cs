using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Infrastructure.Identity.Entities;

namespace RealStateApp.Infrastructure.Identity.Seeds
{
    public class DefaultDeveloperUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var developerUser = new ApplicationUser
            {
                UserName = "developer",
                Email = "developer@example.com",
                FirstName = "Developer",
                LastName = "User",
                IdCard = "87654321",
                IsActive = true
            };

            if (userManager.Users.All(u => u.UserName != developerUser.UserName))
            {
                var result = await userManager.CreateAsync(developerUser, "Developer123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(developerUser, Roles.Developer.ToString());
            }
        }
    }
}
