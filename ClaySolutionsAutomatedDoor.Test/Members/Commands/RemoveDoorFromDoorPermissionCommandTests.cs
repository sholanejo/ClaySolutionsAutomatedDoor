using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Linq.Expressions;

namespace ClaySolutionsAutomatedDoor.Test.Members.Commands
{
    public class RemoveDoorFromDoorPermissionCommandTests
    {
        private readonly Mock<IUnitOfWorkRepository> _mockUnitOfWorkRepository;
        private readonly Mock<ILogger<RemoveDoorFromDoorPermissionCommandHandler>> _mockLogger = new();

        private static readonly Guid DoorId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890");
        private static readonly Guid GroupId = Guid.Parse("1E4993B3-8B6C-40FC-8C68-B31FD3E8C5A4");

        public RemoveDoorFromDoorPermissionCommandTests()
        {
            _mockUnitOfWorkRepository = new Mock<IUnitOfWorkRepository>();
        }

        [Fact]
        public async Task Handle_Should_ReturnPassedResponse_WhenDoorPermissionExists()
        {
            //arrange
            var command = new RemoveDoorFromDoorPermissionCommand
            {
                DoorId = DoorId,
                DoorAccessControlGroupId = GroupId,
                CreatedBy = "system"
            };

            var existingPermission = new DoorPermission
            {
                Id = Guid.NewGuid(),
                DoorId = DoorId,
                DoorAccessControlGroupId = GroupId,
                DateCreated = DateTime.UtcNow
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorPermissionRepository.GetSingleAsync(It.IsAny<Expression<Func<DoorPermission, bool>>>(), default))
                .ReturnsAsync((Expression<Func<DoorPermission, bool>> predicate, CancellationToken _) =>
                {
                    var compiled = predicate.Compile();
                    return compiled(existingPermission) ? existingPermission : null;
                });

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorPermissionRepository.DeleteAsync(It.IsAny<Guid>(), default))
                .Returns(Task.CompletedTask);

            _mockUnitOfWorkRepository
                .Setup(x => x.AuditTrailRepository.InsertAsync(It.IsAny<AuditTrail>(), default))
                .Returns(Task.CompletedTask);

            _mockUnitOfWorkRepository.Setup(x => x.CommitAsync(default)).ReturnsAsync(true);

            var handler = new RemoveDoorFromDoorPermissionCommandHandler(_mockUnitOfWorkRepository.Object, _mockLogger.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.ApiOkMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenDoorPermissionDoesNotExist()
        {
            //arrange
            var command = new RemoveDoorFromDoorPermissionCommand
            {
                DoorId = Guid.NewGuid(),
                DoorAccessControlGroupId = Guid.NewGuid(),
                CreatedBy = "system"
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorPermissionRepository.GetSingleAsync(It.IsAny<Expression<Func<DoorPermission, bool>>>(), default))
                .ReturnsAsync((DoorPermission)null);

            var handler = new RemoveDoorFromDoorPermissionCommandHandler(_mockUnitOfWorkRepository.Object, _mockLogger.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.NoMatchingMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }
    }
}
