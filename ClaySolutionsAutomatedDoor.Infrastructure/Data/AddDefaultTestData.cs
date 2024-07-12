using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Domain.Enums;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Data
{
    public class AddDefaultTestData
    {
        private const string Password = "StrongPassword1$";
        private static Guid GeneralDoorAccessControlGroupId = Guid.Parse("1E4993B3-8B6C-40FC-8C68-B31FD3E8C5A4");
        private static Guid RestrictedDoorAccessControlGroupId = Guid.Parse("5CB3E7C8-FBDA-4D86-AB2D-1111A8D0E4C0");

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
            DoorAccessControlGroupId = RestrictedDoorAccessControlGroupId
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
            DoorAccessControlGroupId = GeneralDoorAccessControlGroupId
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
            DoorAccessControlGroupId = GeneralDoorAccessControlGroupId
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
            DoorAccessControlGroupId = GeneralDoorAccessControlGroupId
        };
        #endregion

        #region Access Groups

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


        public static async Task AddDefaultRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.AdminUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.RegularUser.ToString()));
        }

        public static async Task AddOfficeManagerAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var appUser = await userManager.FindByEmailAsync(OfficeManager.Email!);
            if (appUser == null)
            {
                await userManager.CreateAsync(OfficeManager, Password);

                await userManager.AddToRoleAsync(OfficeManager, Roles.AdminUser.ToString());
            }

            //add claims for officemanager
            var officeManagerRole = await roleManager.FindByNameAsync(Roles.AdminUser.ToString());

        }

        public static async Task AddDirectorAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var appUser = await userManager.FindByEmailAsync(Director.Email!);
            if (appUser == null)
            {
                await userManager.CreateAsync(Director, Password);

                await userManager.AddToRoleAsync(Director, Roles.AdminUser.ToString());
            }

            //seed claims for director
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

            //seed claims for director
        }

        public static async Task AddDoorAccessControlGroupAsync(
           AutomatedDoorDbContext context)
        {
            await context.DoorAccessControlGroup.AddAsync(GeneralDoorAccessControlGroup);
            await context.DoorAccessControlGroup.AddAsync(RestrictedDoorAccessControlGroup);
            await context.SaveChangesAsync();
        }

    }
}
