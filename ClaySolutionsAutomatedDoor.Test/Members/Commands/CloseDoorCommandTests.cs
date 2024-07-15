using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Commands;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace ClaySolutionsAutomatedDoor.Test.Members.Commands
{
    public class CloseDoorCommandTests
    {
        private readonly Mock<IUnitOfWorkRepository> _mockUnitOfWorkRepository;
        private readonly Mock<ILogger<AddDoorCommandHandler>> _mockLogger = new();
        private readonly Mock<IAuditTrailRepository> _mockAuditTrail = new();

        public CloseDoorCommandTests()
        {
            _mockUnitOfWorkRepository = new Mock<IUnitOfWorkRepository>();
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResponse_WhenDoorNameAlreadyExist()
        {
            //arrange
            var command = new AddDoorCommand
            {
                DoorName = "New Door",
                CreatedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",
                Location = "CLay Tech"
            };

            var mockDoor = new Door
            {
                CreatedBy = command.CreatedBy,
                Location = command.Location,
                Name = command.DoorName,
            };

            var mockAuditTrail = new AuditTrail
            {
                DateCreated = DateTime.UtcNow,
                Notes = "",
                PerformedBy = "E586519F-A3D5-4283-BCE5-2292794B4C13",

            };

            _mockUnitOfWorkRepository
               .Setup(x => x.DoorRepository.GetSingleAsync(It.IsAny<Expression<Func<Door, bool>>>()))
               .ReturnsAsync((Expression<Func<Door, bool>> predicate) =>
               {
                   var compiledPredicate = predicate.Compile();
                   return compiledPredicate(mockDoor) ? mockDoor : null;
               });



            var handler = new AddDoorCommandHandler(_mockLogger.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(string.Format(Constants.DoorAlreadyExistMessage, command.DoorName));
            result.StatusCode.ShouldBe((int)StatusCodes.Status409Conflict);
        }
    }
}
