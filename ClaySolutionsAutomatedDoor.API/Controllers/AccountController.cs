using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Features.AccountFeatures.Commands;
using ClaySolutionsAutomatedDoor.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClaySolutionsAutomatedDoor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ISender _sender;

        public AccountController(ISender sender)
        {
            _sender = sender;
        }


        /// <summary>
        /// Logins a user and returns login credentials
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When the user is successfully logged in</response>
        /// <response code="401">when login details are incorrect.</response>
        /// <response code="403">when the user account is inactive.</response>
        /// <response code="423">when the user account is locked.</response>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<LoginResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<LoginResponseDto>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<LoginResponseDto>), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(BaseResponse<LoginResponseDto>), (int)HttpStatusCode.Locked)]
        public async Task<ActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}