using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Features.AccountFeatures.Commands;
using ClaySolutionsAutomatedDoor.Domain.Configurations;
using ClaySolutionsAutomatedDoor.Domain.Entities;
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
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;


        public LoginCommandHandlerTests()
        {
            _mockUserManager = MockHelpers.MockUserManager(TestData.TestUsers);
            _mockRoleManager = MockHelpers.MockRoleManager<IdentityRole>(TestData.TestRoles);
            _mockSignInManager = MockHelpers.MockSignInManager(_mockUserManager);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenUserIsInActive()
        {
            //arrange
            var command = new LoginCommand
            {
                EmailAddress = "ada.lovelace@outlook.com",
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

        [Fact]
        public async Task Handle_Should_ReturnTokenAndSuccessResponse_WhenCredentialsAreValid()
        {
            //arrange
            var command = new LoginCommand
            {
                EmailAddress = TestData.OfficeManager.Email,
                Password = "StrongPassword1$",
            };

            _mockBearerTokenConfig.Setup(x => x.Value).Returns(new BearerTokenConfiguration
            {
                Key = "this-is-a-test-secret-key-that-is-long-enough",
                Issuer = "https://localhost:7149/",
                Audience = "claysolutions",
                ExpiryMinutes = 60
            });

            _mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);

            _mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "AdminUser" });

            _mockUserManager
                .Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<System.Security.Claims.Claim>());

            _mockRoleManager
                .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityRole("AdminUser"));

            _mockRoleManager
                .Setup(x => x.GetClaimsAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(new List<System.Security.Claims.Claim>());

            _mockSignInManager
                .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var handler = new LoginCommandHandler(_mockLogger.Object, _mockUserManager.Object, _mockSignInManager.Object,
                _mockBearerTokenConfig.Object, _mockRoleManager.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.LoginSuccessfulMessage);
            result.Data.AccessToken.ShouldNotBeNullOrEmpty();
            result.Data.UserId.ShouldBe(TestData.OfficeManager.Id);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenPasswordIsWrong()
        {
            //arrange
            var command = new LoginCommand
            {
                EmailAddress = TestData.OfficeManager.Email,
                Password = "WrongPassword1$",
            };

            _mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);

            _mockSignInManager
                .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

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