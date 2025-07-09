using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RealStateApp.Infrastructure.Identity.Entities;
using RealStateApp.Infrastructure.Identity.Seeds;

namespace RealStateApp.Infrastructure.Identity.Extensions
{
    public static class ServiceProviderIdentityExtension
    {
        public static async Task RunIdentitySeedsAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await DefaultRoles.SeedRolesAsync(roleManager);

            await DefaultAdminUser.SeedAsync(userManager);
            await DefaultDeveloperUser.SeedAsync(userManager);
            await DefaultClientUser.SeedAsync(userManager);
            await DefaultAgentUser.SeedAsync(userManager);
        }
    }
}
