using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Features.PermissionFeatures.Queries;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Test.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;
using System.Security.Claims;

namespace ClaySolutionsAutomatedDoor.Test.Members.Queries
{
    public class GetUserPermissionsQueryTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;

        public GetUserPermissionsQueryTests()
        {
            _mockUserManager = MockHelpers.MockUserManager(TestData.TestUsers);
            _mockRoleManager = MockHelpers.MockRoleManager(TestData.TestRoles);
        }

        [Fact]
        public async Task Handle_Should_ReturnClaimsForUser_WhenUserExists()
        {
            //arrange
            var query = new GetUserPermissionsQuery { UserId = TestData.OfficeManager.Id };

            var roleClaims = new List<Claim>
            {
                new Claim("Permission", "Permissions.User.Create"),
                new Claim("Permission", "Permissions.User.View")
            };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);

            _mockUserManager
                .Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<Claim>());

            _mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "AdminUser" });

            _mockRoleManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityRole("AdminUser"));

            _mockRoleManager
                .Setup(x => x.GetClaimsAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(roleClaims);

            var handler = new GetUserPermissionQueryHandler(_mockUserManager.Object, _mockRoleManager.Object);

            //act
            var result = await handler.Handle(query, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.ApiOkMessage);
            result.Data.Count.ShouldBe(2);
            result.Data.ShouldContain(x => x.ClaimValue == "Permissions.User.Create");
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenUserDoesNotExist()
        {
            //arrange
            var query = new GetUserPermissionsQuery { UserId = Guid.NewGuid().ToString() };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            var handler = new GetUserPermissionQueryHandler(_mockUserManager.Object, _mockRoleManager.Object);

            //act
            var result = await handler.Handle(query, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.UserDoesNotExistMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }
    }
}
