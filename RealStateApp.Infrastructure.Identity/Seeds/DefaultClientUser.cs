using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Infrastructure.Identity.Entities;

namespace RealStateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultClientUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            AppUser user = new()
            {
                IdentityCardNumber = "00000000003",
                FirstName = "Juan",
                LastName = "Client",
                Email = "Client@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                UserName = "client",
                RegisteredAt = DateTime.Now 
            };;

            if (await userManager.Users.AllAsync(u => u.Id != user.Id))
            {
                var entityUser = await userManager.FindByEmailAsync(user.Email);
                if(entityUser == null)
                {
                    await userManager.CreateAsync(user, "123Pa$$word!");
                    await userManager.AddToRoleAsync(user, nameof(Roles.Client));
                }
            }
       
        }
    }
}
