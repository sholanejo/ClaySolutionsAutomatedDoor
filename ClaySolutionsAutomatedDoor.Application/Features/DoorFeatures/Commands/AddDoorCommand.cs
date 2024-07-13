using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Commands
{
    public class AddDoorCommand : IRequest<BaseResponse>
    {
        public string DoorName { get; set; }
        public string Location { get; set; }
        public string CreatedBy { get; set; }
    }

    public class AddDoorCommandHandler(ILogger<AddDoorCommandHandler> _logger,
        IUnitOfWorkRepository _unitOfWorkRepository)
        : IRequestHandler<AddDoorCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(AddDoorCommand request, CancellationToken cancellationToken)
        {
            var door = await _unitOfWorkRepository
                .DoorRepository
                .GetSingleAsync(x => x.Name.ToLower().Equals(request.DoorName));

            if (door is not null)
            {
                _logger.LogWarning("Door with name {0} already exist", request.DoorName);
                return BaseResponse.FailedResponse(string.Format(Constants.DoorAlreadyExistMessage, request.DoorName), StatusCodes.Status409Conflict);
            }

            var newDoor = _BuildDoor(request);
            await _unitOfWorkRepository.DoorRepository.InsertAsync(newDoor);

            var notes = string.Format(Constants.AddDoorMessage, request.DoorName);
            var auditTrail = _BuildAuditTrail(notes, request.CreatedBy);
            await _unitOfWorkRepository.AuditTrailRepository.InsertAsync(auditTrail);

            await _unitOfWorkRepository.CommitAsync();

            return BaseResponse.PassedResponse(Constants.ApiOkMessage, StatusCodes.Status200OK);
        }

        private AuditTrail _BuildAuditTrail(string notes, string createdBy)
        {
            return new AuditTrail
            {
                DateCreated = DateTime.Now,
                Notes = notes,
                PerformedBy = createdBy,

            };
        }

        private Door _BuildDoor(AddDoorCommand request)
        {
            return new Door
            {
                DateCreated = DateTime.Now,
                CreatedBy = request.CreatedBy,
                Location = request.Location,
                Name = request.DoorName,
            };
        }
    }
}
