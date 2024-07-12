using ClaySolutionsAutomatedDoor.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Data
{
    public static class AddDefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.AdminUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.RegularUser.ToString()));
        }
    }
}
