using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Commands
{
    public class CloseDoorCommand : IRequest<BaseResponse>
    {
        public Guid DoorId { get; set; }
        public string UserId { get; set; }
    }

    public class CloseDoorCommandHandler(IUnitOfWorkRepository _unitOfWorkRepository,
        ILogger<CloseDoorCommandHandler> _logger,
        UserManager<ApplicationUser> _userManager) : IRequestHandler<CloseDoorCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(CloseDoorCommand request, CancellationToken cancellationToken)
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
                    Notes = string.Format(Constants.AttemptToCloseUnassignedDoor),
                    PerformedBy = request.UserId,
                };

                await _unitOfWorkRepository.AuditTrailRepository.InsertAsync(auditTrail);
                await _unitOfWorkRepository.CommitAsync();

                return BaseResponse.FailedResponse(Constants.AttemptToCloseUnassignedDoor, StatusCodes.Status403Forbidden);
            }

            var auditTrailSuccess = new AuditTrail
            {
                DateCreated = DateTime.Now,
                Notes = string.Format(Constants.CloseDoorSuccessMessage, door.Name),
                PerformedBy = request.UserId,
            };

            await _unitOfWorkRepository.AuditTrailRepository.InsertAsync(auditTrailSuccess);
            await _unitOfWorkRepository.CommitAsync();

            return BaseResponse.PassedResponse(string.Format(Constants.CloseDoorSuccessMessage, door.Name), StatusCodes.Status200OK);
        }
    }
}
