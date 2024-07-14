using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Queries
{
    public class GetSingleDoorQuery : IRequest<BaseResponse<DoorDto>>
    {
        public required Guid DoorId { get; set; }
    }

    public class GetSingleDoorQueryHandler(IUnitOfWorkRepository _unitOfWorkRepository,
        ILogger<GetSingleDoorQueryHandler> _logger) : IRequestHandler<GetSingleDoorQuery, BaseResponse<DoorDto>>
    {
        public async Task<BaseResponse<DoorDto>> Handle(GetSingleDoorQuery request, CancellationToken cancellationToken)
        {
            var door = await _unitOfWorkRepository.DoorRepository.GetByIdAsync(request.DoorId);

            if (door == null)
            {
                return BaseResponse<DoorDto>.FailedResponse(Constants.DoorDoesNotExistMessage, StatusCodes.Status400BadRequest);
            }

            DoorDto doorDto = new()
            {
                DateCreated = door.DateCreated,
                Id = door.Id,
                Location = door.Location,
                Name = door.Name,
            };

            return BaseResponse<DoorDto>.PassedResponse(Constants.ApiOkMessage, doorDto);
        }
    }

}
