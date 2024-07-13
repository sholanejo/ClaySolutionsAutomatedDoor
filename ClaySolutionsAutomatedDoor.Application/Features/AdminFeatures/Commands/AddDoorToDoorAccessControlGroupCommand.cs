using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands
{
    public class AddDoorToDoorAccessControlGroupCommand : IRequest<BaseResponse>
    {
        public Guid DoorId { get; set; }
        public Guid DoorAccessControlGroupId { get; set; }
        public string CreatedBy { get; set; }
    }

    public class AddDoorToDoorAccessControlGroupCommandHandler(ILogger<AddDoorToDoorAccessControlGroupCommandHandler> _logger,
        IUnitOfWorkRepository _unitOfWorkRepository) : IRequestHandler<AddDoorToDoorAccessControlGroupCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(AddDoorToDoorAccessControlGroupCommand request, CancellationToken cancellationToken)
        {
            var doorAccessControlGroup = await _unitOfWorkRepository.DoorAccessControlGroupRepository.GetByIdAsync(request.DoorAccessControlGroupId);
            if (doorAccessControlGroup is null)
            {
                _logger.LogWarning("Door access control group with Id {0} was not found", request.DoorAccessControlGroupId);
                return BaseResponse.FailedResponse(Constants.DoorAccessControlGroupNotFoundMessage, StatusCodes.Status400BadRequest);
            }

            var door = await _unitOfWorkRepository.DoorRepository.GetByIdAsync(request.DoorId);
            if (door is null)
            {
                _logger.LogWarning("Door with Id {0} was not found", request.DoorId);
                return BaseResponse.FailedResponse(Constants.DoorDoesNotExistMessage, StatusCodes.Status400BadRequest);
            }

            var doorAlreadyExistinGroup = await _unitOfWorkRepository
                .DoorPermissionRepository
                .GetSingleAsync(x => x.DoorId.Equals(request.DoorId) && x.DoorAccessControlGroupId.Equals(request.DoorAccessControlGroupId));
            if (doorAlreadyExistinGroup is not null)
            {
                _logger.LogWarning("Cannot add an already added door to group");
                return BaseResponse.FailedResponse(Constants.DoorAlreadyAddedToGroupExistMessage, StatusCodes.Status409Conflict);
            }

            DoorPermission doorPermission = _BuildDoorPermissionObject(request);
            await _unitOfWorkRepository.DoorPermissionRepository.InsertAsync(doorPermission);

            var notes = string.Format(Constants.DoorAddedToGroupMessage, doorAccessControlGroup.GroupName);
            AuditTrail auditTrail = _BuildAuditTrail(notes, request.CreatedBy);
            await _unitOfWorkRepository.AuditTrailRepository.InsertAsync(auditTrail);

            await _unitOfWorkRepository.CommitAsync();

            return BaseResponse.PassedResponse(string.Format(Constants.DoorAddedToGroupMessage, doorAccessControlGroup.GroupName), StatusCodes.Status201Created);
        }

        private DoorPermission _BuildDoorPermissionObject(AddDoorToDoorAccessControlGroupCommand request)
        {
            return new DoorPermission
            {
                DoorId = request.DoorId,
                DateCreated = DateTime.Now,
                DoorAccessControlGroupId = request.DoorAccessControlGroupId,
                CreatedBy = request.CreatedBy,
            };
        }

        private AuditTrail _BuildAuditTrail(string notes, string createdBy)
        {
            return new AuditTrail
            {
                DateCreated = DateTime.Now,
                PerformedBy = createdBy,
                Notes = notes,
            };
        }
    }
}