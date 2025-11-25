using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Infrastructure.Identity.Entities;

namespace RealStateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultAgentUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            AppUser user = new()
            {
                IdentityCardNumber = "00000000002",
                FirstName = "Marcos",
                LastName = "Agent",
                Email = "Agent@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                UserName = "agent",
                RegisteredAt = DateTime.Now 
            };;

            if (await userManager.Users.AllAsync(u => u.Id != user.Id))
            {
                var entityUser = await userManager.FindByEmailAsync(user.Email);
                if(entityUser == null)
                {
                    await userManager.CreateAsync(user, "123Pa$$word!");
                    await userManager.AddToRoleAsync(user, nameof(Roles.Agent));
                }
            }
       
        }
    }
}
