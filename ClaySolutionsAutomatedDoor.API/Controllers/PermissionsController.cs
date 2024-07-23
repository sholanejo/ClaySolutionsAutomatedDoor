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
        /// Get all permission associated with a user 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the request is succcessful</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to get door.</response>
        [HttpGet]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [Route("permissions/{UserId}")]
        [ProducesResponseType(typeof(BaseResponse<List<ClaimDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<List<ClaimDto>>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<List<ClaimDto>>), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> GetUserPermission([FromRoute] GetUserPermissionsQuery query)
        {
            var result = await _sender.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Add Permission to a role 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the role is added</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to get door.</response>
        [HttpPost]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [Route("add-permission-to-role")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> GetDoor([FromBody] AddPermissionToRoleCommand command)
        {
            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Add a user to a role 
        /// </summary>
        /// <returns></returns>
        /// <response code="201">When the user is successfully added to a role</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to add user to role.</response>
        [HttpPost]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [Route("add-user-to-role")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> AddUserToRole([FromBody] AddUserToRoleCommand command)
        {
            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}
