using MediatR;
using Microsoft.AspNetCore.Mvc;

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


    }
}
