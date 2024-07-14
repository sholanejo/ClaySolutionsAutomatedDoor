using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands
{
    public class DeActivateUserCommand : IRequest<BaseResponse>
    {
        public Guid UserId { get; set; }
    }

    public class DeActivateUserCommandHandler(UserManager<ApplicationUser> _userManager,
        ILogger<DeActivateUserCommandHandler> _logger,
        IUnitOfWorkRepository _unitOfWorkRepository) : IRequestHandler<DeActivateUserCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(DeActivateUserCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (applicationUser == null)
            {
                _logger.LogWarning("Application User with Id {0} does not exist", request.UserId);
                return BaseResponse.FailedResponse(Constants.UserDoesNotExistMessage, StatusCodes.Status400BadRequest);
            }

            if (!applicationUser.IsActive)
            {
                return BaseResponse.FailedResponse(Constants.DeactivateUserFailedMessage, StatusCodes.Status400BadRequest);
            }

            applicationUser.IsActive = false;
            await _userManager.UpdateAsync(applicationUser);
            await _unitOfWorkRepository.CommitAsync();

            return BaseResponse.PassedResponse(Constants.DeactivateUserMessage, StatusCodes.Status200OK);
        }
    }
}
