using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Domain.Enums;
using ClaySolutionsAutomatedDoor.Infrastructure.Helpers;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Data
{
    public static class AddDefaultUsers
    {
        private const string Password = "StrongPassword1$";
        private static Guid GeneralDoorAccessControlGroupId = Guid.Parse("1E4993B3-8B6C-40FC-8C68-B31FD3E8C5A4");
        private static Guid RestrictedDoorAccessControlGroupId = Guid.Parse("5CB3E7C8-FBDA-4D86-AB2D-1111A8D0E4C0");

        #region Door Access Control Groups

        static DoorAccessControlGroup RestrictedDoorAccessControlGroup = new()
        {
            Id = RestrictedDoorAccessControlGroupId,
            GroupName = "Restricted Group",
            DateCreated = DateTime.Now,
            CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
        };

        static DoorAccessControlGroup GeneralDoorAccessControlGroup = new()
        {
            Id = GeneralDoorAccessControlGroupId,
            GroupName = "General Group",
            DateCreated = DateTime.Now,
            CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
        };
        #endregion

        #region Users

        static ApplicationUser OfficeManager = new()
        {
            Id = "E586519F-A3D5-4283-BCE5-2292794B4C13",
            FirstName = "Shola",
            LastName = "Nejo",
            Email = "sholanejo@live.com",
            UserName = "sholanejo@live.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            IsActive = true,
            DoorAccessControlGroupId = RestrictedDoorAccessControlGroupId,
            CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
            CreatedDate = DateTime.Now
        };

        static ApplicationUser Director = new()
        {
            FirstName = "Timothy",
            LastName = "Johnson",
            Email = "timothyjohnson@outlook.com",
            UserName = "timothyjohnson@outlook.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            IsActive = true,
            DoorAccessControlGroupId = GeneralDoorAccessControlGroupId,
            CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
            CreatedDate = DateTime.Now
        };

        static ApplicationUser Employee = new()
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@gmail.com",
            UserName = "jane.doe@gmail.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            IsActive = true,
            DoorAccessControlGroupId = GeneralDoorAccessControlGroupId,
            CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
            CreatedDate = DateTime.Now
        };

        static ApplicationUser InActiveUser = new()
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada.lovelace@hotmail.com",
            UserName = "ada.lovelace@outlook.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            IsActive = false,
            DoorAccessControlGroupId = GeneralDoorAccessControlGroupId,
            CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
            CreatedDate = DateTime.Now
        };
        #endregion


        public static async Task AddDefaultDoorAccessControlGroupsAsync(
           AutomatedDoorDbContext context)
        {
            await context.DoorAccessControlGroup.AddAsync(RestrictedDoorAccessControlGroup);
            await context.DoorAccessControlGroup.AddAsync(GeneralDoorAccessControlGroup);
            await context.SaveChangesAsync();
        }


        public static async Task AddOfficeManagerAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var appUser = await userManager.FindByEmailAsync(OfficeManager.Email!);
            if (appUser == null)
            {
                await userManager.CreateAsync(OfficeManager, Password);

                await userManager.AddToRoleAsync(OfficeManager, Roles.AdminUser.ToString());
                await userManager.AddToRoleAsync(OfficeManager, Roles.RegularUser.ToString());
            }
            await roleManager.AddClaimsForAdminUser();

        }

        public static async Task AddDirectorAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var appUser = await userManager.FindByEmailAsync(Director.Email!);
            if (appUser == null)
            {
                await userManager.CreateAsync(Director, Password);

                await userManager.AddToRoleAsync(Director, Roles.AdminUser.ToString());
                await userManager.AddToRoleAsync(Director, Roles.RegularUser.ToString());
            }
            await roleManager.AddClaimsForAdminUser();
        }

        public static async Task AddEmployeeAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var appUser = await userManager.FindByEmailAsync(Employee.Email!);
            if (appUser == null)
            {
                await userManager.CreateAsync(Employee, Password);

                await userManager.AddToRoleAsync(Employee, Roles.RegularUser.ToString());
            }

            //seed claims for director
        }

        public static async Task AddInActiveUser(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var appUser = await userManager.FindByEmailAsync(InActiveUser.Email!);
            if (appUser == null)
            {
                await userManager.CreateAsync(InActiveUser, Password);

                await userManager.AddToRoleAsync(InActiveUser, Roles.RegularUser.ToString());
            }
        }

        private static async Task AddClaimsForAdminUser(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.AdminUser.ToString());
            await roleManager.AddPermissionClaimAsync(adminRole!, "User");
            await roleManager.AddPermissionClaimAsync(adminRole!, "AccessControl");
        }

        private static async Task AddPermissionClaimAsync(this RoleManager<IdentityRole> roleManager,
            IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }
}
