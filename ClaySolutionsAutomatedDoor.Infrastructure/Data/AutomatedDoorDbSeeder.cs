using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Data
{
    public static class AutomatedDoorDbSeeder
    {
        public static async Task SeedDataAsync(AutomatedDoorDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await dbContext.Database.EnsureCreatedAsync();

            if (!dbContext.Users.Any())
            {
                Log.Logger.Information("No user found in database, seeding data started...");

                await AddDefaultUsers.AddDefaultDoorAccessControlGroupsAsync(dbContext);
                await AddDefaultRoles.SeedAsync(roleManager);
                await AddDefaultUsers.AddInActiveUser(userManager, roleManager);
                await AddDefaultUsers.AddEmployeeAsync(userManager, roleManager);
                await AddDefaultUsers.AddOfficeManagerAsync(userManager, roleManager);
                await AddDefaultUsers.AddDirectorAsync(userManager, roleManager);

                Log.Logger.Information("Data seeding data completed successfully...");
            }
        }
    }
}
