using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Infrastructure.Data;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace ClaySolutionsAutomatedDoor.API.Extensions
{
    public static class SeedDbData
    {
        public static async Task SetupDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<AutomatedDoorDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await AutomatedDoorDbSeeder.SeedDataAsync(dbContext, userManager, roleManager);
        }
    }
}
