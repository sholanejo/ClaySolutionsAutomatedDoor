using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Query;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Test.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace ClaySolutionsAutomatedDoor.Test.Members.Queries
{
    public class GetAuditTrailQueryTests
    {
        private readonly Mock<IUnitOfWorkRepository> _mockUnitOfWorkRepository;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<ILogger<GetAuditTrailQueryHandler>> _mockLogger = new();

        public GetAuditTrailQueryTests()
        {
            _mockUnitOfWorkRepository = new Mock<IUnitOfWorkRepository>();
            _mockUserManager = MockHelpers.MockUserManager(TestData.TestUsers);
        }

        [Fact]
        public async Task Handle_Should_ReturnPaginatedAuditTrail_WhenUserExists()
        {
            //arrange
            var userId = TestData.OfficeManager.Id;
            var query = new GetAuditTrailQuery { UserId = userId, Page = 1, PageSize = 10 };

            var auditTrails = new List<AuditTrail>
            {
                new() { PerformedBy = userId, Notes = "Opened Main Entrance", DateCreated = DateTime.UtcNow },
                new() { PerformedBy = userId, Notes = "Opened Storage Room", DateCreated = DateTime.UtcNow }
            }.AsQueryable();

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);

            _mockUnitOfWorkRepository
                .Setup(x => x.AuditTrailRepository.GetQueryAsync(It.IsAny<Func<AuditTrail, bool>>(), default))
                .ReturnsAsync(auditTrails);

            var handler = new GetAuditTrailQueryHandler(_mockLogger.Object, _mockUnitOfWorkRepository.Object, _mockUserManager.Object);

            //act
            var result = await handler.Handle(query, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.ApiOkMessage);
            result.Data.totalItemCount.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenUserDoesNotExist()
        {
            //arrange
            var query = new GetAuditTrailQuery { UserId = Guid.NewGuid().ToString(), Page = 1, PageSize = 10 };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            var handler = new GetAuditTrailQueryHandler(_mockLogger.Object, _mockUnitOfWorkRepository.Object, _mockUserManager.Object);

            //act
            var result = await handler.Handle(query, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.UserDoesNotExistMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }
    }
}
