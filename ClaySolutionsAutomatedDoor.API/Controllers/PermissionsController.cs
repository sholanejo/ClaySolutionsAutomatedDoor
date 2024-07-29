using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Features.PermissionFeatures.Commands;
using ClaySolutionsAutomatedDoor.Application.Features.PermissionFeatures.Queries;
using ClaySolutionsAutomatedDoor.Domain.Dtos;
using ClaySolutionsAutomatedDoor.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClaySolutionsAutomatedDoor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly ISender _sender;

        public PermissionsController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Get all permissions associated with a user 
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns></returns>
        /// <response code="200">When the request is successful</response>
        /// <response code="401">If JWT token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to get door.</response>
        [HttpGet("{userId}/permissions")]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [ProducesResponseType(typeof(BaseResponse<List<ClaimDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<List<ClaimDto>>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<List<ClaimDto>>), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> GetUserPermission([FromRoute] string userId)
        {
            var query = new GetUserPermissionsQuery { UserId = userId };
            var result = await _sender.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Add permission to a role 
        /// </summary>
        /// <param name="command">The command to add permission to role</param>
        /// <param name="roleId">The Id of the rolee</param>
        /// <returns></returns>
        /// <response code="200">When the role is added</response>
        /// <response code="401">If JWT token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to get door.</response>
        [HttpPost("roles/{roleId}/permissions")]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> AddPermissionToRole([FromRoute] string roleId, [FromBody] AddPermissionToRoleCommand command)
        {
            command.RoleId = roleId;
            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Add a user to a role 
        /// </summary>
        /// <param name="command">The command to add user to role</param>
        /// <param name="roleId">The Id of the role</param>
        /// <returns></returns>
        /// <response code="201">When the user is successfully added to a role</response>
        /// <response code="401">If JWT token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to add user to role.</response>
        [HttpPost("roles/{roleId}/users")]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> AddUserToRole([FromRoute] string roleId, [FromBody] AddUserToRoleCommand command)
        {
            command.RoleId = roleId;
            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}
