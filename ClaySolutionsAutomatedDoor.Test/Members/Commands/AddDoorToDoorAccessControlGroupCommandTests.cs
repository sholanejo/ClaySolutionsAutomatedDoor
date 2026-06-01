using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Test.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Linq.Expressions;

namespace ClaySolutionsAutomatedDoor.Test.Members.Commands
{
    public class AddDoorToDoorAccessControlGroupCommandTests
    {
        private readonly Mock<IUnitOfWorkRepository> _mockUnitOfWorkRepository;
        private readonly Mock<ILogger<AddDoorToDoorAccessControlGroupCommandHandler>> _mockLogger = new();

        private static readonly Guid DoorId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890");
        private static readonly Guid GroupId = Guid.Parse("1E4993B3-8B6C-40FC-8C68-B31FD3E8C5A4");

        private readonly Door _testDoor = new()
        {
            Id = DoorId,
            Name = "Main Entrance",
            Location = "Ground Floor",
            DateCreated = DateTime.UtcNow,
            CreatedBy = "system"
        };

        public AddDoorToDoorAccessControlGroupCommandTests()
        {
            _mockUnitOfWorkRepository = new Mock<IUnitOfWorkRepository>();
        }

        [Fact]
        public async Task Handle_Should_ReturnPassedResponse_WhenDoorIsSuccessfullyAdded()
        {
            //arrange
            var command = new AddDoorToDoorAccessControlGroupCommand
            {
                DoorId = DoorId,
                DoorAccessControlGroupId = GroupId,
                CreatedBy = "system"
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync(TestData.GeneralDoorAccessControlGroup);

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync(_testDoor);

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorPermissionRepository.GetSingleAsync(It.IsAny<Expression<Func<DoorPermission, bool>>>(), default))
                .ReturnsAsync((DoorPermission)null);

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorPermissionRepository.InsertAsync(It.IsAny<DoorPermission>(), default))
                .Returns(Task.CompletedTask);

            _mockUnitOfWorkRepository
                .Setup(x => x.AuditTrailRepository.InsertAsync(It.IsAny<AuditTrail>(), default))
                .Returns(Task.CompletedTask);

            _mockUnitOfWorkRepository.Setup(x => x.CommitAsync(default)).ReturnsAsync(true);

            var handler = new AddDoorToDoorAccessControlGroupCommandHandler(_mockLogger.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(true);
            result.StatusCode.ShouldBe((int)StatusCodes.Status201Created);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenGroupDoesNotExist()
        {
            //arrange
            var command = new AddDoorToDoorAccessControlGroupCommand
            {
                DoorId = DoorId,
                DoorAccessControlGroupId = Guid.NewGuid(),
                CreatedBy = "system"
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync((DoorAccessControlGroup)null);

            var handler = new AddDoorToDoorAccessControlGroupCommandHandler(_mockLogger.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.DoorAccessControlGroupNotFoundMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenDoorDoesNotExist()
        {
            //arrange
            var command = new AddDoorToDoorAccessControlGroupCommand
            {
                DoorId = Guid.NewGuid(),
                DoorAccessControlGroupId = GroupId,
                CreatedBy = "system"
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync(TestData.GeneralDoorAccessControlGroup);

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync((Door)null);

            var handler = new AddDoorToDoorAccessControlGroupCommandHandler(_mockLogger.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.DoorDoesNotExistMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenDoorAlreadyInGroup()
        {
            //arrange
            var command = new AddDoorToDoorAccessControlGroupCommand
            {
                DoorId = DoorId,
                DoorAccessControlGroupId = GroupId,
                CreatedBy = "system"
            };

            var existingPermission = new DoorPermission
            {
                DoorId = DoorId,
                DoorAccessControlGroupId = GroupId,
                DateCreated = DateTime.UtcNow
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync(TestData.GeneralDoorAccessControlGroup);

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync(_testDoor);

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorPermissionRepository.GetSingleAsync(It.IsAny<Expression<Func<DoorPermission, bool>>>(), default))
                .ReturnsAsync((Expression<Func<DoorPermission, bool>> predicate, CancellationToken _) =>
                {
                    var compiled = predicate.Compile();
                    return compiled(existingPermission) ? existingPermission : null;
                });

            var handler = new AddDoorToDoorAccessControlGroupCommandHandler(_mockLogger.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.DoorAlreadyAddedToGroupExistMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status409Conflict);
        }
    }
}
