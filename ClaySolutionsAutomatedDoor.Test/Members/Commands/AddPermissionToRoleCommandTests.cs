using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Features.PermissionFeatures.Commands;
using ClaySolutionsAutomatedDoor.Test.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;
using System.Security.Claims;

namespace ClaySolutionsAutomatedDoor.Test.Members.Commands
{
    public class AddPermissionToRoleCommandTests
    {
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;

        public AddPermissionToRoleCommandTests()
        {
            _mockRoleManager = MockHelpers.MockRoleManager(TestData.TestRoles);
        }




        [Fact]
        public async Task Handle_Should_ReturnPassedResponse_WhenPermissionIsSuccessfullyAddedToRole()
        {
            //arrange
            var command = new AddPermissionToRoleCommand
            {
                ClaimType = "Permission",
                ClaimValue = "TestPermission",
                RoleId = "e586519f-a3d5-4283-bce5-2292794b4c13"
            };

            var testIdentityRole = new IdentityRole
            {
                Id = "1e4993b3-8b6c-40fc-8c68-b31fd3e8c5a4",
                ConcurrencyStamp = "E586519F-A3D5-4283-BCE5-2292794B4C13",
                Name = "Name",
                NormalizedName = "NAME"
            };


            _mockRoleManager
               .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
               .ReturnsAsync(testIdentityRole);

            _mockRoleManager
              .Setup(x => x.AddClaimAsync(It.IsAny<IdentityRole>(), It.IsAny<Claim>()))
              .ReturnsAsync(IdentityResult.Success);

            var handler = new AddPermissionToRoleCommandHandler(_mockRoleManager.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.ApiOkMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status200OK);
        }
    }
}
