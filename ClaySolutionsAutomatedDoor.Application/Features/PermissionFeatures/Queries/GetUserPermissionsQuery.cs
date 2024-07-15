using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Domain.Dtos;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ClaySolutionsAutomatedDoor.Application.Features.PermissionFeatures.Queries
{
    public class GetUserPermissionsQuery : IRequest<BaseResponse<List<ClaimDto>>>
    {
        [Required]
        public string UserId { get; set; }
    }

    public class GetUserPermissionQueryHandler(UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager) : IRequestHandler<GetUserPermissionsQuery, BaseResponse<List<ClaimDto>>>
    {
        public async Task<BaseResponse<List<ClaimDto>>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            var applicationUser = await _userManager.FindByIdAsync(request.UserId);

            if (applicationUser == null)
            {
                return BaseResponse<List<ClaimDto>>.FailedResponse(Constants.UserDoesNotExistMessage, StatusCodes.Status400BadRequest);
            }

            var allClaims = new List<Claim>();
            var roleClaims = new List<Claim>();

            var userClaims = await _userManager.GetClaimsAsync(applicationUser);

            // Get user roles
            var roles = await _userManager.GetRolesAsync(applicationUser);

            // Get role claims
            foreach (var role in roles)
            {
                var identityRole = await _roleManager.FindByNameAsync(role);
                if (identityRole != null)
                {
                    roleClaims = _roleManager.GetClaimsAsync(identityRole).Result.ToList();
                    allClaims.AddRange(roleClaims);
                }
            }
            allClaims.AddRange(userClaims);

            var userclaimsDto = allClaims.Select(x => new ClaimDto
            {
                ClaimType = x.Type,
                ClaimValue = x.Value,
            }).ToList();

            return BaseResponse<List<ClaimDto>>.PassedResponse(Constants.ApiOkMessage, userclaimsDto, StatusCodes.Status200OK);
        }
    }
}
