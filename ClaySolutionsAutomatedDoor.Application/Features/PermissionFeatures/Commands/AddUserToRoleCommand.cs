using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClaySolutionsAutomatedDoor.Application.Features.PermissionFeatures.Commands
{
    public class AddUserToRoleCommand : IRequest<BaseResponse>
    {
        [Required(ErrorMessage = "User Id Is required")]
        public string UserId { get; set; }
        [JsonIgnore]
        [Required(ErrorMessage = "Role Id Is required")]
        public string RoleId { get; set; }
    }

    public class AddUserToRoleCommandHandler(UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager)
        : IRequestHandler<AddUserToRoleCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return BaseResponse.FailedResponse(Constants.UserDoesNotExistMessage, StatusCodes.Status400BadRequest);
            }

            var role = await _roleManager.FindByIdAsync(request.RoleId);

            if (role is null)
            {
                return BaseResponse.FailedResponse("Role does not exist", StatusCodes.Status400BadRequest);
            }

            var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
            if (isInRole)
            {
                return BaseResponse.FailedResponse($"User is already in the specified role {role.Name}", StatusCodes.Status400BadRequest);
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                string errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return BaseResponse.FailedResponse(errorMessage, StatusCodes.Status500InternalServerError);
            }

            return BaseResponse.PassedResponse(Constants.ApiOkMessage, StatusCodes.Status201Created);
        }
    }
}