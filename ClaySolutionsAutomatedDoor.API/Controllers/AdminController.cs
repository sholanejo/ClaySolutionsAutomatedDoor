using ClaySolutionsAutomatedDoor.API.Utility;
using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Utility;
using ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands;
using ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Query;
using ClaySolutionsAutomatedDoor.Application.Features.DoorAccessControlFeatures.Commands;
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
    public class AdminController : ControllerBase
    {
        private readonly ISender _sender;

        public AdminController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <returns></returns>
        /// <response code="201">When the user is successfully created</response>
        /// <response code="401">If JWT token provided is invalid.</response>
        /// <response code="403">If caller does not have permission to create user.</response>
        [HttpPost("users")]
        [Authorize(Policy = "UserCreation")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> AddUser([FromBody] AddNewUserDto addNewUserRequest)
        {
            var createdBy = User.Identity.GetUserId();

            var command = new AddApplicationUserCommand
            {
                DoorAccessControlGroupId = addNewUserRequest.AccessGroupId,
                CreatedBy = createdBy,
                Email = addNewUserRequest.Email,
                FirstName = addNewUserRequest.FirstName,
                LastName = addNewUserRequest.LastName,
                Password = addNewUserRequest.Password,
            };

            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Creates a door access control group
        /// </summary>
        /// <returns></returns>
        /// <response code="201">When the access control group is successfully created</response>
        /// <response code="401">If JWT token provided is invalid.</response>
        /// <response code="403">If caller does not have permission to create access control group.</response>
        [HttpPost("access-control-groups")]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> AddDoorAccessControlGroup([FromBody] AddAccessControlGroupDto request)
        {
            var createdBy = User.Identity.GetUserId();

            var command = new AddDoorAccessControlGroupCommand
            {
                GroupName = request.GroupName,
                ActorId = createdBy,
            };

            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Assigns a door to an access control group
        /// </summary>
        /// <returns></returns>
        /// <response code="201">When the door is successfully assigned</response>
        /// <response code="401">If JWT token provided is invalid.</response>
        /// <response code="403">If caller does not have permission to assign door.</response>
        [HttpPost("access-control-groups/{groupId}/doors")]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> AddDoorPermission([FromRoute] Guid groupId, [FromBody] AddDoorPermissionDto request)
        {
            var createdBy = User.Identity.GetUserId();

            var command = new AddDoorToDoorAccessControlGroupCommand
            {
                DoorAccessControlGroupId = groupId,
                CreatedBy = createdBy,
                DoorId = request.DoorId,
            };

            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Deactivate User
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the user is successfully deactivated</response>
        /// <response code="401">If JWT token provided is invalid.</response>
        /// <response code="403">If caller does not have permission to deactivate user.</response>
        [HttpPatch("users/{userId}/deactivate")]
        [Authorize(Policy = "CanChangeUserActiveState")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> DeActivateUser([FromRoute] Guid userId)
        {
            var command = new DeActivateUserCommand { UserId = userId };
            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Activate User
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the user is successfully activated</response>
        /// <response code="401">If JWT token provided is invalid.</response>
        /// <response code="403">If caller does not have permission to activate user.</response>
        [HttpPatch("users/{userId}/activate")]
        [Authorize(Policy = "CanChangeUserActiveState")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> ActivateUser([FromRoute] Guid userId)
        {
            var command = new ActivateUserCommand { UserId = userId };
            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Removes door from access control group
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the door is successfully removed</response>
        /// <response code="401">If JWT token provided is invalid.</response>
        /// <response code="403">If caller does not have permission to remove door.</response>
        [HttpDelete("access-control-groups/{groupId}/doors/{doorId}")]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> RemoveDoorFromDoorAccessGroup([FromRoute] Guid groupId, [FromRoute] Guid doorId)
        {
            var createdBy = User.Identity.GetUserId();

            var command = new RemoveDoorFromDoorPermissionCommand
            {
                DoorAccessControlGroupId = groupId,
                CreatedBy = createdBy,
                DoorId = doorId
            };

            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get audit trail of specified user
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the request is successful</response>
        /// <response code="401">If JWT token provided is invalid.</response>
        /// <response code="403">If caller does not have permission to view audit trail.</response>
        [HttpGet("audit-trail")]
        [Authorize(Policy = "CanReadAuditTrail")]
        [ProducesResponseType(typeof(BaseResponse<PaginatedParameter<AuditTrailDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<PaginatedParameter<AuditTrailDto>>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<PaginatedParameter<AuditTrailDto>>), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> GetAuditTrail([FromQuery] GetAuditTrailQuery request)
        {
            var query = new GetAuditTrailQuery
            {
                UserId = request.UserId,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var result = await _sender.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get door access control group
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the request is successful</response>
        /// <response code="401">If JWT token provided is invalid.</response>
        /// <response code="403">If caller does not have permission to view access control group.</response>
        [HttpGet("access-control-groups")]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [ProducesResponseType(typeof(BaseResponse<PaginatedParameter<DoorAccessControlGroupDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<PaginatedParameter<DoorAccessControlGroupDto>>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<PaginatedParameter<DoorAccessControlGroupDto>>), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> GetDoorAccessControlGroup([FromQuery] GetDoorAccessControlGroupQuery request)
        {
            var result = await _sender.Send(request);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Updates the user access control group
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the request is successful</response>
        /// <response code="401">If JWT token provided is invalid.</response>
        /// <response code="403">If caller does not have permission to update access control group.</response>
        [HttpPatch("users/{userId}/access-control-groups")]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> UpdateDoorAccessControlGroup([FromRoute] string userId, [FromBody] UpdateUserDoorAccessControlGroupCommand request)
        {
            request.UserId = userId;
            var result = await _sender.Send(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}