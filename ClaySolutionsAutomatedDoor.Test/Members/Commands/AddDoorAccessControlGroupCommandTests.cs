using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Features.DoorAccessControlFeatures.Commands;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Test.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Linq.Expressions;

namespace ClaySolutionsAutomatedDoor.Test.Members.Commands
{
    public class AddDoorAccessControlGroupCommandTests
    {
        private readonly Mock<IUnitOfWorkRepository> _mockUnitOfWorkRepository;
        private readonly Mock<ILogger<AddAccessControlGroupCommandHandler>> _mockLogger = new();

        public AddDoorAccessControlGroupCommandTests()
        {
            _mockUnitOfWorkRepository = new Mock<IUnitOfWorkRepository>();
        }

        [Fact]
        public async Task Handle_Should_ReturnPassedResponse_WhenGroupNameIsUnique()
        {
            //arrange
            var command = new AddDoorAccessControlGroupCommand
            {
                GroupName = "New Group",
                ActorId = "system"
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository.GetSingleAsync(It.IsAny<Expression<Func<DoorAccessControlGroup, bool>>>(), default))
                .ReturnsAsync((DoorAccessControlGroup)null);

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository.InsertAsync(It.IsAny<DoorAccessControlGroup>(), default))
                .Returns(Task.CompletedTask);

            _mockUnitOfWorkRepository
                .Setup(x => x.AuditTrailRepository.InsertAsync(It.IsAny<AuditTrail>(), default))
                .Returns(Task.CompletedTask);

            _mockUnitOfWorkRepository.Setup(x => x.CommitAsync(default)).ReturnsAsync(true);

            var handler = new AddAccessControlGroupCommandHandler(_mockLogger.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.ApiOkMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status201Created);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenGroupNameAlreadyExists()
        {
            //arrange
            var command = new AddDoorAccessControlGroupCommand
            {
                GroupName = TestData.GeneralDoorAccessControlGroup.GroupName,
                ActorId = "system"
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository.GetSingleAsync(It.IsAny<Expression<Func<DoorAccessControlGroup, bool>>>(), default))
                .ReturnsAsync(TestData.GeneralDoorAccessControlGroup);

            var handler = new AddAccessControlGroupCommandHandler(_mockLogger.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.AddDoorAccessGroupFailureMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status409Conflict);
        }
    }
}
