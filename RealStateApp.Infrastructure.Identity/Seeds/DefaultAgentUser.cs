using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Infrastructure.Identity.Entities;

namespace RealStateApp.Infrastructure.Identity.Seeds
{
    public class DefaultAgentUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var agentUser = new ApplicationUser
            {
                UserName = "agent",
                Email = "agent@example.com",
                FirstName = "Agent",
                LastName = "User",
                PhoneNumber = "0987654321",
                IsActive = false
            };

            if (userManager.Users.All(u => u.UserName != agentUser.UserName))
            {
                var result = await userManager.CreateAsync(agentUser, "Agent123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(agentUser, Roles.Agent.ToString());
                }
            }
        }
    }
}
