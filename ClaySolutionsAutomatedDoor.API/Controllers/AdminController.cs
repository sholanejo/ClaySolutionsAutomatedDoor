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
        /// This will additionally log the activity.
        /// </summary>
        /// <returns></returns>
        /// <response code="201">When the user is successfully created</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to create user.</response>
        [HttpPost]
        [Route("create-user")]
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
        /// This will additionally log the activity.
        /// </summary>
        /// <returns></returns>
        /// <response code="201">When the user is successfully created</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to create user.</response>
        [HttpPost]
        [Route("add-access-control-group")]
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
        /// This will additionally log the activity.
        /// </summary>
        /// <returns></returns>
        /// <response code="201">When the user is successfully created</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to create user.</response>
        [HttpPost]
        [Route("assign-door-to-access-control-group")]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> AddDoorPermission([FromBody] AddDoorPermissionDto request)
        {
            var createdBy = User.Identity.GetUserId();

            var command = new AddDoorToDoorAccessControlGroupCommand
            {
                DoorAccessControlGroupId = request.DoorAccessControlGroupId,
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
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to create user.</response>
        [HttpPatch]
        [Route("deactivate-user")]
        [Authorize(Policy = "CanChangeUserActiveState")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> DeActivateUser([FromBody] DeActivateUserCommand command)
        {
            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Activate User
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the user is successfully Activated</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to create user.</response>
        [HttpPatch]
        [Route("activate-user")]
        [Authorize(Policy = "CanChangeUserActiveState")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> ActivateUser([FromBody] ActivateUserCommand command)
        {
            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Removes door from access control groupr
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the user is successfully Activated</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to create user.</response>
        [HttpDelete]
        [Route("remove-door-from-access-control-group")]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> RemoveDoorFromDoorAccessGroup([FromBody] RemoveDoorFromDoorPermissionDto request)
        {
            var createdBy = User.Identity.GetUserId();

            var command = new RemoveDoorFromDoorPermissionCommand { DoorAccessControlGroupId = request.DoorAccessControlGroupId, CreatedBy = createdBy, DoorId = request.DoorId };

            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get audit trail of specified user
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the request is successful</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to create user.</response>
        [HttpGet]
        [Route("audit-trail")]
        [Authorize(Policy = "CanReadAuditTrail")]
        [ProducesResponseType(typeof(BaseResponse<PaginatedParameter<AuditTrailDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<PaginatedParameter<AuditTrailDto>>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<PaginatedParameter<AuditTrailDto>>), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> GetAuditTrail([FromQuery] GetAuditTrailQuery request)
        {
            var query = new GetAuditTrailQuery { UserId = request.UserId, Page = request.Page, PageSize = request.PageSize };

            var result = await _sender.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get door access control group
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the request is successful</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to create user.</response>
        [HttpGet]
        [Route("door-access-control-group")]
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
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to create user.</response>
        [HttpPatch]
        [Route("user-door-access-control-group")]
        [Authorize(Roles = nameof(Roles.AdminUser))]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> UpdateDoorAccessControlGroup([FromBody] UpdateUserDoorAccessControlGroupCommand request)
        {
            var result = await _sender.Send(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
