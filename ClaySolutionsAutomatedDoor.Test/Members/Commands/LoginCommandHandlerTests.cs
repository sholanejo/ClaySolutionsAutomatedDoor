using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Features.AccountFeatures.Commands;
using ClaySolutionsAutomatedDoor.Domain.Configurations;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Test.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;

namespace ClaySolutionsAutomatedDoor.Test.Members.Commands
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private readonly Mock<ILogger<LoginCommandHandler>> _mockLogger = new();
        private readonly Mock<IOptions<BearerTokenConfiguration>> _mockBearerTokenConfig = new();


        public LoginCommandHandlerTests()
        {
            _mockUserManager = MockHelpers.MockUserManager(TestData.TestUsers);
            _mockRoleManager = MockHelpers.MockRoleManager<IdentityRole>(TestData.TestRoles);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenUserIsInActive()
        {
            //arrange
            var command = new LoginCommand
            {
                EmailAddress = "ada.lovelace@hotmail.com",
                Password = "StrongPassword1$",
            };

            _mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.InActiveUser);
            var handler = new LoginCommandHandler(_mockLogger.Object, _mockUserManager.Object, _mockSignInManager.Object,
                _mockBearerTokenConfig.Object, _mockRoleManager.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.InActiveUserLoginAttemptMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task Handle_Should_ReturnUnauthorizedResponse_WhenUserDoesNotExist()
        {
            // arrange
            var command = new LoginCommand
            {
                EmailAddress = "emaildoesnotexist@hotmail.com",
                Password = "StrongPassword1$",
            };

            var handler = new LoginCommandHandler(_mockLogger.Object, _mockUserManager.Object, _mockSignInManager.Object,
             _mockBearerTokenConfig.Object, _mockRoleManager.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.FailedLoginAttemptMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }
    }
}