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
    public class AddApplicationUserCommandHandlerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IUnitOfWorkRepository> _mockUnitOfWorkRepository;
        private readonly Mock<ILogger<AddApplicationUserCommandHandler>> _mockLogger = new();
        private readonly Mock<IDoorAccessControlGroupRepository> _mockDoorAccessGroup = new();
        private readonly Mock<IAuditTrailRepository> _mockAuditTrail = new();

        public AddApplicationUserCommandHandlerTests()
        {
            _mockUserManager = MockHelpers.MockUserManager(TestData.TestUsers);
            _mockUnitOfWorkRepository = new Mock<IUnitOfWorkRepository>();
            _mockUnitOfWorkRepository.Setup(x => x.DoorAccessControlGroupRepository).Returns(_mockDoorAccessGroup.Object);
            _mockUnitOfWorkRepository.Setup(x => x.AuditTrailRepository).Returns(_mockAuditTrail.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenEmailIsNotUnique()
        {
            //arrange
            var command = new AddApplicationUserCommand
            {
                DoorAccessControlGroupId = TestData.OfficeManager.DoorAccessControlGroupId,
                FirstName = TestData.OfficeManager.FirstName,
                CreatedBy = TestData.OfficeManager.CreatedBy,
                Email = TestData.OfficeManager.Email,
                LastName = TestData.OfficeManager.LastName,
                Password = "Password1$",
            };

            _mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);
            var handler = new AddApplicationUserCommandHandler(_mockUserManager.Object, _mockUnitOfWorkRepository.Object, _mockLogger.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.UserAlreadyExistsMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status409Conflict);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenDoorAccessControlGroupIdIsNotFound()
        {
            //arrange
            var command = new AddApplicationUserCommand
            {
                DoorAccessControlGroupId = Guid.NewGuid(),
                FirstName = TestData.OfficeManager.FirstName,
                CreatedBy = TestData.OfficeManager.CreatedBy,
                Email = TestData.OfficeManager.Email,
                LastName = TestData.OfficeManager.LastName,
                Password = "Password1$",
            };

            DoorAccessControlGroup controlGroup = null!;

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(controlGroup);
            var handler = new AddApplicationUserCommandHandler(_mockUserManager.Object, _mockUnitOfWorkRepository.Object, _mockLogger.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.DoorAccessControlGroupNotFoundMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResponse_WhenEmailIsUnique()
        {
            //arrange
            var command = new AddApplicationUserCommand
            {
                DoorAccessControlGroupId = TestData.OfficeManager.DoorAccessControlGroupId,
                FirstName = "UniqueFirstName",
                CreatedBy = TestData.OfficeManager.CreatedBy,
                Email = "uniqueemailaddress@outlook.com",
                LastName = "UniqueLastName",
                Password = "Password1$",
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(TestData.RestrictedDoorAccessControlGroup);

            var handler = new AddApplicationUserCommandHandler(_mockUserManager.Object, _mockUnitOfWorkRepository.Object, _mockLogger.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.ApiOkMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status201Created);
        }
    }
}
