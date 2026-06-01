using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Query;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Test.Shared;
using Moq;
using Shouldly;

namespace ClaySolutionsAutomatedDoor.Test.Members.Queries
{
    public class GetDoorAccessControlGroupQueryTests
    {
        private readonly Mock<IUnitOfWorkRepository> _mockUnitOfWorkRepository;

        public GetDoorAccessControlGroupQueryTests()
        {
            _mockUnitOfWorkRepository = new Mock<IUnitOfWorkRepository>();
        }

        [Fact]
        public async Task Handle_Should_ReturnPaginatedGroups()
        {
            //arrange
            var query = new GetDoorAccessControlGroupQuery { Page = 1, PageSize = 10 };

            var groups = new List<DoorAccessControlGroup>
            {
                new()
                {
                    Id = TestData.GeneralDoorAccessControlGroup.Id,
                    GroupName = TestData.GeneralDoorAccessControlGroup.GroupName,
                    DateCreated = DateTime.UtcNow,
                    DoorPermission = new List<DoorPermission>(),
                    Users = new List<ApplicationUser>()
                },
                new()
                {
                    Id = TestData.RestrictedDoorAccessControlGroup.Id,
                    GroupName = TestData.RestrictedDoorAccessControlGroup.GroupName,
                    DateCreated = DateTime.UtcNow,
                    DoorPermission = new List<DoorPermission>(),
                    Users = new List<ApplicationUser>()
                }
            }.AsQueryable();

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository.GetAllQuery())
                .Returns(groups);

            var handler = new GetDoorAccessControlGroupQueryHandler(_mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(query, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.ApiOkMessage);
            result.Data.totalItemCount.ShouldBe(2);
        }
    }
}
