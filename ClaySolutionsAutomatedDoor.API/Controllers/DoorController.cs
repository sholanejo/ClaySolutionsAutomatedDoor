using ClaySolutionsAutomatedDoor.API.Utility;
using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Commands;
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
        [Authorize]
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



    }
}
