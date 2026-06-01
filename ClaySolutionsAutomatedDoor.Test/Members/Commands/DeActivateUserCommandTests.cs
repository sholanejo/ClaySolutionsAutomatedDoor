using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Test.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace ClaySolutionsAutomatedDoor.Test.Members.Commands
{
    public class DeActivateUserCommandTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IUnitOfWorkRepository> _mockUnitOfWorkRepository;
        private readonly Mock<ILogger<DeActivateUserCommandHandler>> _mockLogger = new();

        public DeActivateUserCommandTests()
        {
            _mockUserManager = MockHelpers.MockUserManager(TestData.TestUsers);
            _mockUnitOfWorkRepository = new Mock<IUnitOfWorkRepository>();
        }

        [Fact]
        public async Task Handle_Should_ReturnPassedResponse_WhenUserIsActive()
        {
            //arrange
            var command = new DeActivateUserCommand { UserId = Guid.NewGuid() };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);
            var handler = new DeActivateUserCommandHandler(_mockUserManager.Object, _mockLogger.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.DeactivateUserMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenUserDoesNotExist()
        {
            //arrange
            var command = new DeActivateUserCommand { UserId = Guid.NewGuid() };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);
            var handler = new DeActivateUserCommandHandler(_mockUserManager.Object, _mockLogger.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.UserDoesNotExistMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenUserIsAlreadyInactive()
        {
            //arrange
            var command = new DeActivateUserCommand { UserId = Guid.NewGuid() };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.InActiveUser);
            var handler = new DeActivateUserCommandHandler(_mockUserManager.Object, _mockLogger.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.DeactivateUserFailedMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }
    }
}
