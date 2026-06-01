using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Features.PermissionFeatures.Commands;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Test.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;

namespace ClaySolutionsAutomatedDoor.Test.Members.Commands
{
    public class AddUserToRoleCommandTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;

        private static readonly IdentityRole AdminRole = new()
        {
            Id = "role-id-1",
            Name = "AdminUser",
            NormalizedName = "ADMINUSER"
        };

        public AddUserToRoleCommandTests()
        {
            _mockUserManager = MockHelpers.MockUserManager(TestData.TestUsers);
            _mockRoleManager = MockHelpers.MockRoleManager(TestData.TestRoles);
        }

        [Fact]
        public async Task Handle_Should_ReturnPassedResponse_WhenUserIsSuccessfullyAddedToRole()
        {
            //arrange
            var command = new AddUserToRoleCommand
            {
                UserId = TestData.OfficeManager.Id,
                RoleId = AdminRole.Id
            };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);

            _mockRoleManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(AdminRole);

            _mockUserManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            _mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var handler = new AddUserToRoleCommandHandler(_mockUserManager.Object, _mockRoleManager.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.ApiOkMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status201Created);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenUserDoesNotExist()
        {
            //arrange
            var command = new AddUserToRoleCommand
            {
                UserId = Guid.NewGuid().ToString(),
                RoleId = AdminRole.Id
            };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            var handler = new AddUserToRoleCommandHandler(_mockUserManager.Object, _mockRoleManager.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.UserDoesNotExistMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenRoleDoesNotExist()
        {
            //arrange
            var command = new AddUserToRoleCommand
            {
                UserId = TestData.OfficeManager.Id,
                RoleId = Guid.NewGuid().ToString()
            };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);

            _mockRoleManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityRole)null);

            var handler = new AddUserToRoleCommandHandler(_mockUserManager.Object, _mockRoleManager.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe("Role does not exist");
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenUserAlreadyInRole()
        {
            //arrange
            var command = new AddUserToRoleCommand
            {
                UserId = TestData.OfficeManager.Id,
                RoleId = AdminRole.Id
            };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);

            _mockRoleManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(AdminRole);

            _mockUserManager
                .Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var handler = new AddUserToRoleCommandHandler(_mockUserManager.Object, _mockRoleManager.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldContain(AdminRole.Name);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }
    }
}
