using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Commands;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Test.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Linq.Expressions;

namespace ClaySolutionsAutomatedDoor.Test.Members.Commands
{
    public class OpenDoorCommandTests
    {
        private readonly Mock<IUnitOfWorkRepository> _mockUnitOfWorkRepository;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<ILogger<OpenDoorCommandHandler>> _mockLogger = new();
        private readonly Mock<IAuditTrailRepository> _mockAuditTrail = new();

        public OpenDoorCommandTests()
        {
            _mockUnitOfWorkRepository = new Mock<IUnitOfWorkRepository>();
            _mockUserManager = MockHelpers.MockUserManager(TestData.TestUsers);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenDoorDoesNotExist()
        {
            //arrange
            var command = new OpenDoorCommand
            {
                DoorId = Guid.Parse("1e4993b3-8b6c-40fc-8c68-b31fd3e8c5a4"),
                UserId = "E586519F-A3D5-4283-BCE5-2292794B4C13"
            };

            var door = new Door
            {
                DateCreated = DateTime.UtcNow,
                CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
                Location = "SPain",
                Name = "Fresh Door",
                Id = Guid.Parse("E586519F-A3D5-4283-BCE5-2292794B4C13")
            };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);

            _mockUnitOfWorkRepository
               .Setup(x => x.DoorRepository.GetByIdAsync(It.IsAny<string>()))
               .ReturnsAsync(door);

            var handler = new OpenDoorCommandHandler(_mockUnitOfWorkRepository.Object, _mockLogger.Object, _mockUserManager.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.DoorDoesNotExistMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenUserIsNotAssignToDoor()
        {
            //arrange
            var command = new OpenDoorCommand
            {
                DoorId = Guid.Parse("E586519F-A3D5-4283-BCE5-2292794B4C13"),
                UserId = "E586519F-A3D5-4283-BCE5-2292794B4C13"
            };

            var mockDoorPermission = new DoorPermission
            {
                DateCreated = DateTime.UtcNow,
                CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
                DoorAccessControlGroupId = Guid.Parse("1e4993b3-8b6c-40fc-8c68-b31fd3e8c5a4"),
                DoorId = Guid.Parse("08dca349-a002-4de5-8f23-bf37ab89ee1a"),
            };

            var door = new Door
            {
                DateCreated = DateTime.UtcNow,
                CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
                Location = "SPain",
                Name = "Fresh Door",
                Id = Guid.Parse("E586519F-A3D5-4283-BCE5-2292794B4C13")
            };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);

            _mockUnitOfWorkRepository
               .Setup(x => x.DoorRepository.GetByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(door);

            _mockUnitOfWorkRepository
               .Setup(x => x.DoorPermissionRepository.GetSingleAsync(It.IsAny<Expression<Func<DoorPermission, bool>>>()))
               .ReturnsAsync((Expression<Func<DoorPermission, bool>> predicate) =>
               {
                   var compiledPredicate = predicate.Compile();
                   return compiledPredicate(mockDoorPermission) ? mockDoorPermission : null;
               });

            _mockUnitOfWorkRepository
                .Setup(x => x.AuditTrailRepository.InsertAsync(It.IsAny<AuditTrail>())).Returns(Task.CompletedTask);

            var handler = new OpenDoorCommandHandler(_mockUnitOfWorkRepository.Object, _mockLogger.Object, _mockUserManager.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.AttemptToOpenUnassignedDoor);
            result.StatusCode.ShouldBe((int)StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task Handle_Should_ReturnPassedResponse_WhenUserIsAssignToDoor()
        {
            //arrange
            var command = new OpenDoorCommand
            {
                DoorId = Guid.Parse("E586519F-A3D5-4283-BCE5-2292794B4C13"),
                UserId = "E586519F-A3D5-4283-BCE5-2292794B4C13"
            };

            var mockDoorPermission = new DoorPermission
            {
                DateCreated = DateTime.UtcNow,
                CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
                DoorAccessControlGroupId = Guid.Parse("5cb3e7c8-fbda-4d86-ab2d-1111a8d0e4c0"),
                DoorId = Guid.Parse("E586519F-A3D5-4283-BCE5-2292794B4C13"),
            };

            var door = new Door
            {
                DateCreated = DateTime.UtcNow,
                CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
                Location = "SPain",
                Name = "Fresh Door",
                Id = Guid.Parse("E586519F-A3D5-4283-BCE5-2292794B4C13")
            };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);

            _mockUnitOfWorkRepository
               .Setup(x => x.DoorRepository.GetByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(door);

            _mockUnitOfWorkRepository
               .Setup(x => x.DoorPermissionRepository.GetSingleAsync(It.IsAny<Expression<Func<DoorPermission, bool>>>()))
               .ReturnsAsync((Expression<Func<DoorPermission, bool>> predicate) =>
               {
                   var compiledPredicate = predicate.Compile();
                   return compiledPredicate(mockDoorPermission) ? mockDoorPermission : null;
               });

            _mockUnitOfWorkRepository
                .Setup(x => x.AuditTrailRepository.InsertAsync(It.IsAny<AuditTrail>())).Returns(Task.CompletedTask);

            var handler = new OpenDoorCommandHandler(_mockUnitOfWorkRepository.Object, _mockLogger.Object, _mockUserManager.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(string.Format(Constants.OpenDoorSuccessMessage, door.Name));
            result.StatusCode.ShouldBe((int)StatusCodes.Status200OK);
        }
    }
}