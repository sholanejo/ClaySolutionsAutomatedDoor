using ClaySolutionsAutomatedDoor.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace ClaySolutionsAutomatedDoor.Application.Features.PermissionFeatures.Commands
{
    public class AddPermissionToRoleCommand : IRequest<BaseResponse>
    {
        [Required(ErrorMessage = "The {0} field is required")]
        [JsonIgnore]
        public string RoleId { get; set; }
        [Required]
        public string ClaimType { get; set; }
        [Required]
        public string ClaimValue { get; set; }
    }

    public class AddPermissionToRoleCommandHandler(RoleManager<IdentityRole> _roleManager) : IRequestHandler<AddPermissionToRoleCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(AddPermissionToRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role == null)
            {
                return BaseResponse.FailedResponse(Constants.RoleNotFOundMessage, StatusCodes.Status400BadRequest);
            }

            var claim = new Claim(request.ClaimType, request.ClaimValue);
            var result = await _roleManager.AddClaimAsync(role, claim);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));

                return BaseResponse.FailedResponse(errors, StatusCodes.Status400BadRequest);
            }
            return BaseResponse.PassedResponse(Constants.ApiOkMessage, StatusCodes.Status200OK);
        }
    }
}