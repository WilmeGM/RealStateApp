using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Infrastructure.Identity.Entities;

namespace RealStateApp.Infrastructure.Identity.Seeds
{
    public class DefaultClientUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var clientUser = new ApplicationUser
            {
                UserName = "client",
                Email = "client@example.com",
                FirstName = "Client",
                LastName = "User",
                PhoneNumber = "1234567890",
                IsActive = false
            };

            if (userManager.Users.All(u => u.UserName != clientUser.UserName))
            {
                var result = await userManager.CreateAsync(clientUser, "Client123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(clientUser, Roles.Client.ToString());
                }
            }
        }
    }
}
