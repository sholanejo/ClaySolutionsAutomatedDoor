using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Test.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;

namespace ClaySolutionsAutomatedDoor.Test.Members.Commands
{
    public class UpdateUserDoorAccessControlGroupCommandTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IUnitOfWorkRepository> _mockUnitOfWorkRepository;

        private static readonly Guid NewGroupId = Guid.Parse("5CB3E7C8-FBDA-4D86-AB2D-1111A8D0E4C0");

        public UpdateUserDoorAccessControlGroupCommandTests()
        {
            _mockUserManager = MockHelpers.MockUserManager(TestData.TestUsers);
            _mockUnitOfWorkRepository = new Mock<IUnitOfWorkRepository>();
        }

        [Fact]
        public async Task Handle_Should_ReturnPassedResponse_WhenGroupIsSuccessfullyUpdated()
        {
            //arrange
            var command = new UpdateUserDoorAccessControlGroupCommand
            {
                UserId = TestData.OfficeManager.Id,
                NewAccessControlGroupId = NewGroupId
            };

            var userWithDifferentGroup = new ApplicationUser
            {
                Id = TestData.OfficeManager.Id,
                Email = TestData.OfficeManager.Email,
                IsActive = true,
                DoorAccessControlGroupId = Guid.Parse("1E4993B3-8B6C-40FC-8C68-B31FD3E8C5A4")
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync(TestData.RestrictedDoorAccessControlGroup);

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(userWithDifferentGroup);

            _mockUserManager
                .Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUnitOfWorkRepository.Setup(x => x.CommitAsync(default)).ReturnsAsync(true);

            var handler = new UpdateUserDoorControlGroupCommandHandler(_mockUserManager.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(true);
            result.Message.ShouldBe(Constants.ApiOkMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenGroupDoesNotExist()
        {
            //arrange
            var command = new UpdateUserDoorAccessControlGroupCommand
            {
                UserId = TestData.OfficeManager.Id,
                NewAccessControlGroupId = Guid.NewGuid()
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync((DoorAccessControlGroup)null);

            var handler = new UpdateUserDoorControlGroupCommandHandler(_mockUserManager.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.DoorAccessControlGroupNotFoundMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenUserDoesNotExist()
        {
            //arrange
            var command = new UpdateUserDoorAccessControlGroupCommand
            {
                UserId = Guid.NewGuid().ToString(),
                NewAccessControlGroupId = NewGroupId
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync(TestData.RestrictedDoorAccessControlGroup);

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser)null);

            var handler = new UpdateUserDoorControlGroupCommandHandler(_mockUserManager.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.UserDoesNotExistMessage);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailedResponse_WhenUserAlreadyBelongsToGroup()
        {
            //arrange
            var command = new UpdateUserDoorAccessControlGroupCommand
            {
                UserId = TestData.OfficeManager.Id,
                NewAccessControlGroupId = TestData.OfficeManager.DoorAccessControlGroupId
            };

            _mockUnitOfWorkRepository
                .Setup(x => x.DoorAccessControlGroupRepository.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync(TestData.RestrictedDoorAccessControlGroup);

            _mockUserManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(TestData.OfficeManager);

            var handler = new UpdateUserDoorControlGroupCommandHandler(_mockUserManager.Object, _mockUnitOfWorkRepository.Object);

            //act
            var result = await handler.Handle(command, default);

            //assert
            result.Status.ShouldBe(false);
            result.Message.ShouldBe(Constants.UpdateAccessControlGroup);
            result.StatusCode.ShouldBe((int)StatusCodes.Status400BadRequest);
        }
    }
}
