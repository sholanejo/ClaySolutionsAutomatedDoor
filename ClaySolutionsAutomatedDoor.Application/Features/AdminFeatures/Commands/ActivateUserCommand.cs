using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands
{
    public class ActivateUserCommand : IRequest<BaseResponse>
    {
        public Guid UserId { get; set; }
    }

    public class ActivateUserCommandHandler(UserManager<ApplicationUser> _userManager,
        ILogger<DeActivateUserCommandHandler> _logger,
        IUnitOfWorkRepository _unitOfWorkRepository) : IRequestHandler<ActivateUserCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (applicationUser == null)
            {
                _logger.LogWarning("Application User with Id {0} does not exist", request.UserId);
                return BaseResponse.FailedResponse(Constants.UserDoesNotExistMessage, StatusCodes.Status400BadRequest);
            }

            if (applicationUser.IsActive)
            {
                return BaseResponse.FailedResponse(Constants.ActivateUserFailedMessage, StatusCodes.Status400BadRequest);
            }

            applicationUser.IsActive = true;
            await _userManager.UpdateAsync(applicationUser);
            await _unitOfWorkRepository.CommitAsync();

            return BaseResponse.PassedResponse(Constants.ActivateUserMessage, StatusCodes.Status200OK);
        }
    }
}
