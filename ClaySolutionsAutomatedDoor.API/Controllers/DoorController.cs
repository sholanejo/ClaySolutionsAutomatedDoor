using ClaySolutionsAutomatedDoor.API.Utility;
using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Utility;
using ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Commands;
using ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Queries;
using ClaySolutionsAutomatedDoor.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClaySolutionsAutomatedDoor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DoorController : ControllerBase
    {

        private readonly ISender _sender;

        public DoorController(ISender sender)
        {
            _sender = sender;
        }


        /// <summary>
        /// Creates a new door 
        /// This will additionally log the activity.
        /// </summary>
        /// <returns></returns>
        /// <response code="201">When the door is successfully created</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to create user.</response>
        /// <response code="409">If the door already exists.</response>
        [HttpPost]
        [Authorize(Policy = "CanAddDoor")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> AddDoor([FromBody] AddDoorRequestDto request)
        {
            var createdBy = User.Identity.GetUserId();

            var command = new AddDoorCommand
            {
                DoorName = request.DoorName,
                CreatedBy = createdBy,
                Location = request.Location,
            };

            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }


        /// <summary>
        /// Opens a door 
        /// This will additionally log the activity.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the door is successfully opened</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to open door.</response>
        [HttpPost]
        [Route("open/{DoorId}")]
        [Authorize]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> OpenDoor([FromRoute] Guid DoorId)
        {
            var userId = User.Identity.GetUserId();
            var command = new OpenDoorCommand { DoorId = DoorId, UserId = userId };
            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get a door 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the door is returned</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to get door.</response>
        [HttpGet]
        [Authorize]
        [Route("{DoorId}")]
        [ProducesResponseType(typeof(BaseResponse<DoorDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<DoorDto>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<DoorDto>), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> GetDoor([FromRoute] GetSingleDoorQuery query)
        {
            var result = await _sender.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get all doors 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the doors are returned</response>
        /// <response code="401">If jwt token provided is invalid.</response>
        /// <response code="403">If caller does not have the permission to get door.</response>
        [HttpGet]
        [Route("doors")]
        [Authorize]
        [ProducesResponseType(typeof(BaseResponse<PaginatedParameter<DoorDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<PaginatedParameter<DoorDto>>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<PaginatedParameter<DoorDto>>), (int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> GetAllDoors([FromQuery] GetDoorsQuery query)
        {
            var result = await _sender.Send(query);
            return StatusCode(result.StatusCode, result);
        }
    }
}