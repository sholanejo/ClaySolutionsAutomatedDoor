using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Queries;
using ClaySolutionsAutomatedDoor.Domain.Dtos;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace ClaySolutionsAutomatedDoor.Test.Members.Queries
{
    public class GetSingleDoorQueryTests
    {
        private readonly Mock<IUnitOfWorkRepository> _mockUnitOfWorkRepository;
        private readonly Mock<ILogger<GetSingleDoorQueryHandler>> _mockLogger = new();

        private static readonly Guid DoorId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890");

        private readonly Door _testDoor = new()
        {
            Id = DoorId,
            Name = "Main Entrance",
            Location = "Ground Floor",
            DateCreated = DateTime.UtcNow,
            CreatedBy = "system"
        };

        public GetSingleDoorQueryTests()
        {
            _mockUnitOfWorkRepository = new Mock<IUnitOfWorkRepository>();
        }

        [Fact]
        public async Task Handle_Should_ReturnDoor_WhenDoorExists()
        {
            //arrange
            var query = new GetSingleDoorQuery { DoorId = DoorId };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync(_testDoor);

            var handler = new GetSingleDoorQueryHandler(_mockUnitOfWorkRepository.Object, _mockLogger.Object);

            //act
            var result = await handler.Handle(query, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.ApiOkMessage);
            result.Data.ShouldNotBeNull();
            result.Data.Id.ShouldBe(DoorId);
            result.Data.Name.ShouldBe("Main Entrance");
            result.Data.Location.ShouldBe("Ground Floor");
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenDoorDoesNotExist()
        {
            //arrange
            var query = new GetSingleDoorQuery { DoorId = Guid.NewGuid() };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync((Door)null);

            var handler = new GetSingleDoorQueryHandler(_mockUnitOfWorkRepository.Object, _mockLogger.Object);

            //act
            var result = await handler.Handle(query, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.DoorDoesNotExistMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }
    }
}
