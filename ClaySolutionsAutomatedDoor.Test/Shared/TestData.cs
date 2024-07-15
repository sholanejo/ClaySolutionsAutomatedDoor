using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ClaySolutionsAutomatedDoor.Test.Shared
{
    internal class TestData
    {
        private const string Password = "StrongPassword1$";
        private static readonly Guid GeneralDoorAccessControlGroupId = Guid.Parse("1E4993B3-8B6C-40FC-8C68-B31FD3E8C5A4");
        private static readonly Guid RestrictedDoorAccessControlGroupId = Guid.Parse("5CB3E7C8-FBDA-4D86-AB2D-1111A8D0E4C0");

        #region Users

        public static ApplicationUser OfficeManager = new()
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

        public static ApplicationUser InActiveUser = new()
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

        #region Door Access Control Groups

        public static DoorAccessControlGroup RestrictedDoorAccessControlGroup = new()
        {
            Id = RestrictedDoorAccessControlGroupId,
            GroupName = "Restricted Group",
            DateCreated = DateTime.Now,
            CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
        };

        public static DoorAccessControlGroup GeneralDoorAccessControlGroup = new()
        {
            Id = GeneralDoorAccessControlGroupId,
            GroupName = "General Group",
            DateCreated = DateTime.Now,
            CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
        };
        #endregion

        public static List<ApplicationUser> TestUsers = new()
        {
            InActiveUser,
            Employee,
            OfficeManager,
            Director
        };

        public static List<IdentityRole> TestRoles = new()
        {
            new IdentityRole(Roles.AdminUser.ToString()),
            new IdentityRole(Roles.RegularUser.ToString()),
        };
    }
}
