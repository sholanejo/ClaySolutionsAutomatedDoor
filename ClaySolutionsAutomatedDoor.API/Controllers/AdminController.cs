using ClaySolutionsAutomatedDoor.API.Utility;
using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands;
using ClaySolutionsAutomatedDoor.Application.Features.DoorAccessControlFeatures.Commands;
using ClaySolutionsAutomatedDoor.Domain.Dtos;
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
        [Route("door-access-control-group")]
        [Authorize]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> AddDoorAccessControlGroup([FromBody] AddAccessControlGroupDto request)
        {
            var createdBy = User.Identity.GetUserId();

            var command = new AddAccessControlGroupCommand
            {
                GroupName = request.GroupName,
                ActorId = createdBy,
            };

            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}
