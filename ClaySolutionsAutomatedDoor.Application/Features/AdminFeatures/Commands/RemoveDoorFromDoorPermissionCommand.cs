using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands
{
    public class RemoveDoorFromDoorPermissionCommand : IRequest<BaseResponse>
    {
        public required Guid DoorId { get; set; }
        public required Guid DoorAccessControlGroupId { get; set; }
        public string CreatedBy { get; set; }
    }

    public class RemoveDoorFromDoorPermissionCommandHandler(IUnitOfWorkRepository _unitOfWorkRepository,
        ILogger<RemoveDoorFromDoorPermissionCommandHandler> _logger) : IRequestHandler<RemoveDoorFromDoorPermissionCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(RemoveDoorFromDoorPermissionCommand request, CancellationToken cancellationToken)
        {
            var doorPermission = await _unitOfWorkRepository
                .DoorPermissionRepository
                .GetSingleAsync(x => x.DoorId.Equals(request.DoorId) && x.DoorAccessControlGroupId.Equals(request.DoorAccessControlGroupId));
            if (doorPermission == null)
            {
                return BaseResponse.FailedResponse(Constants.NoMatchingMessage, StatusCodes.Status400BadRequest);
            }

            await _unitOfWorkRepository.DoorPermissionRepository.DeleteAsync(doorPermission.Id);

            var notes = string.Format(Constants.AccessGroupDeletedMessage,
                doorPermission.DoorId, doorPermission.DoorAccessControlGroupId);
            AuditTrail auditTrail = new()
            {
                DateCreated = DateTime.Now,
                PerformedBy = request.CreatedBy,
                Notes = notes
            };

            await _unitOfWorkRepository.CommitAsync();

            return BaseResponse.PassedResponse(Constants.ApiOkMessage, StatusCodes.Status200OK);
        }
    }
}
