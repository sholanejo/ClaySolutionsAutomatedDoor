using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ClaySolutionsAutomatedDoor.Application.Features.DoorAccessControlFeatures.Commands
{
    public class AddDoorAccessControlGroupCommand : IRequest<BaseResponse>
    {
        public string GroupName { get; set; }
        public string ActorId { get; set; }
    }

    public class AddAccessControlGroupCommandHandler(ILogger<AddAccessControlGroupCommandHandler> _logger,
        IUnitOfWorkRepository _unitOfWorkRepository) : IRequestHandler<AddDoorAccessControlGroupCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(AddDoorAccessControlGroupCommand request, CancellationToken cancellationToken)
        {
            var doorAccessControlGroup = await _unitOfWorkRepository
                .DoorAccessControlGroupRepository
                .GetSingleAsync(x => x.GroupName.ToLower().Equals(request.GroupName));
            if (doorAccessControlGroup is not null)
            {
                _logger.LogWarning("Door Access control group cannot be added because the name : {0} already exists", request.GroupName);
                return BaseResponse.FailedResponse(Constants.AddDoorAccessGroupFailureMessage, StatusCodes.Status409Conflict);
            }

            var newDoorAccessControlGroup = _BuildDoorAccessControlGroup(request);
            await _unitOfWorkRepository.DoorAccessControlGroupRepository.InsertAsync(newDoorAccessControlGroup);

            var notes = string.Format(Constants.DoorAccessGroupActivityMessage, request.GroupName);
            var auditTrail = _BuildAuditTrail(notes, request.ActorId);
            await _unitOfWorkRepository.AuditTrailRepository.InsertAsync(auditTrail);

            await _unitOfWorkRepository.CommitAsync();

            _logger.LogInformation("Access group with name : {0} was successfully added", request.GroupName);

            return BaseResponse.PassedResponse(Constants.ApiOkMessage, StatusCodes.Status201Created);
        }

        private DoorAccessControlGroup _BuildDoorAccessControlGroup(AddDoorAccessControlGroupCommand request)
        {
            return new DoorAccessControlGroup()
            {
                DateCreated = DateTime.Now,
                GroupName = request.GroupName,
                CreatedBy = request.ActorId,
            };
        }

        private AuditTrail _BuildAuditTrail(string notes, string actorId)
        {
            return new AuditTrail
            {
                DateCreated = DateTime.Now,
                PerformedBy = actorId,
                Notes = notes
            };
        }
    }
}
