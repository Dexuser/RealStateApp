using Microsoft.AspNetCore.Identity;
using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Admin)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Client)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Agent)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Developer)));
        }
    }
}
