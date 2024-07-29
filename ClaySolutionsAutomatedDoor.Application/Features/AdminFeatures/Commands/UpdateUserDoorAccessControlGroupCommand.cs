using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands
{
    public class UpdateUserDoorAccessControlGroupCommand : IRequest<BaseResponse>
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public required Guid NewAccessControlGroupId { get; set; }
    }

    public class UpdateUserDoorControlGroupCommandHandler(UserManager<ApplicationUser> _userManager,
        IUnitOfWorkRepository _UnitOfWorkRepository) : IRequestHandler<UpdateUserDoorAccessControlGroupCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(UpdateUserDoorAccessControlGroupCommand request, CancellationToken cancellationToken)
        {
            var newAccessControlGroup = await _UnitOfWorkRepository.DoorAccessControlGroupRepository
                .GetByIdAsync(request.NewAccessControlGroupId);

            if (newAccessControlGroup == null)
            {
                return BaseResponse.FailedResponse(Constants.DoorAccessControlGroupNotFoundMessage, StatusCodes.Status400BadRequest);
            }

            var applicationUser = await _userManager.FindByIdAsync(request.UserId);
            if (applicationUser == null)
            {
                return BaseResponse.FailedResponse(Constants.UserDoesNotExistMessage, StatusCodes.Status400BadRequest);
            }

            if (applicationUser.DoorAccessControlGroupId.Equals(request.NewAccessControlGroupId))
            {
                return BaseResponse.FailedResponse(Constants.UpdateAccessControlGroup, StatusCodes.Status400BadRequest);
            }

            applicationUser.DoorAccessControlGroupId = request.NewAccessControlGroupId;
            await _userManager.UpdateAsync(applicationUser);
            await _UnitOfWorkRepository.CommitAsync();

            return BaseResponse.PassedResponse(Constants.ApiOkMessage, StatusCodes.Status200OK);
        }
    }
}
