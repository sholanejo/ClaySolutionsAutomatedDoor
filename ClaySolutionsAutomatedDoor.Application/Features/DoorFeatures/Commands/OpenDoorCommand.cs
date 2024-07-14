using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Commands
{
    public class OpenDoorCommand : IRequest<BaseResponse>
    {
        public string UserId { get; set; }
        public Guid DoorId { get; set; }
    }

    public class OpenDoorCommandHandler(IUnitOfWorkRepository _unitOfWorkRepository,
        ILogger<OpenDoorCommandHandler> _logger,
        UserManager<ApplicationUser> _userManager
        ) : IRequestHandler<OpenDoorCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(OpenDoorCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _userManager.FindByIdAsync(request.UserId);
            if (applicationUser == null)
            {
                _logger.LogWarning(Constants.UserDoesNotExistMessage);
                return BaseResponse.FailedResponse(Constants.UserDoesNotExistMessage, StatusCodes.Status400BadRequest);
            }

            var door = await _unitOfWorkRepository.DoorRepository.GetByIdAsync(request.DoorId);
            if (door is null)
            {
                _logger.LogWarning(Constants.DoorDoesNotExistMessage);
                return BaseResponse.FailedResponse(Constants.DoorDoesNotExistMessage, StatusCodes.Status400BadRequest);
            }

            var doorPermission = await _unitOfWorkRepository
                .DoorPermissionRepository
                .GetSingleAsync
                (x => x.DoorAccessControlGroupId == applicationUser.DoorAccessControlGroupId && x.DoorId == request.DoorId);

            if (doorPermission is null)
            {
                var auditTrail = new AuditTrail()
                {
                    DateCreated = DateTime.Now,
                    Notes = string.Format(Constants.AttemptToOpenUnassignedDoor),
                    PerformedBy = request.UserId,
                };

                await _unitOfWorkRepository.AuditTrailRepository.InsertAsync(auditTrail);
                await _unitOfWorkRepository.CommitAsync();

                return BaseResponse.FailedResponse(Constants.AttemptToOpenUnassignedDoor, StatusCodes.Status403Forbidden);
            }

            var auditTrailSuccess = new AuditTrail
            {
                DateCreated = DateTime.Now,
                Notes = string.Format(Constants.OpenDoorSuccessMessage, door.Name),
                PerformedBy = request.UserId,
            };

            await _unitOfWorkRepository.AuditTrailRepository.InsertAsync(auditTrailSuccess);
            await _unitOfWorkRepository.CommitAsync();

            return BaseResponse.PassedResponse(string.Format(Constants.OpenDoorSuccessMessage, door.Name), StatusCodes.Status200OK);
        }
    }
}
