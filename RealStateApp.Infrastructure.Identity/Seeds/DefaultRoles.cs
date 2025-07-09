using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Application.Enums;

namespace RealStateApp.Infrastructure.Identity.Seeds
{
    public class DefaultRoles
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.Admin.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

            if (!await roleManager.RoleExistsAsync(Roles.Developer.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Developer.ToString()));

            if (!await roleManager.RoleExistsAsync(Roles.Client.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Client.ToString()));

            if (!await roleManager.RoleExistsAsync(Roles.Agent.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Agent.ToString()));
        }
    }
}
