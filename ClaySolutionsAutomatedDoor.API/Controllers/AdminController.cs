using ClaySolutionsAutomatedDoor.Application.Features.Admin.Commands;
using ClaySolutionsAutomatedDoor.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        //[Authorize(Policy = "UserCreation")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddUser([FromBody] AddNewUserDto addNewUserRequest)
        {
            //var createdBy = User.Identity.GetUserId();
            var createdBy = "E586519F-A3D5-4283-BCE5-2292794B4C13";

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
    }
}
