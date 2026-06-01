using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Queries;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using Moq;
using Shouldly;

namespace ClaySolutionsAutomatedDoor.Test.Members.Queries
{
    public class GetDoorsQueryTests
    {
        private readonly Mock<IUnitOfWorkRepository> _mockUnitOfWorkRepository;

        public GetDoorsQueryTests()
        {
            _mockUnitOfWorkRepository = new Mock<IUnitOfWorkRepository>();
        }

        [Fact]
        public async Task Handle_Should_ReturnPaginatedDoors_WithCorrectNameMapping()
        {
            //arrange
            var query = new GetDoorsQuery { Page = 1, PageSize = 10 };

            var doors = new List<Door>
            {
                new() { Id = Guid.NewGuid(), Name = "Main Entrance", Location = "Ground Floor", DateCreated = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), Name = "Storage Room", Location = "First Floor", DateCreated = DateTime.UtcNow }
            }.AsQueryable();

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorRepository.GetAllQuery())
                .Returns(doors);

            var handler = new GetDoorQueryHandler(_mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(query, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.ApiOkMessage);
            result.Data.totalItemCount.ShouldBe(2);
            result.Data.items.ShouldAllBe(d => d.Name != d.Location);
        }
    }
}
