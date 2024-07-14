using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Common.Utility;
using ClaySolutionsAutomatedDoor.Domain.Dtos;
using MediatR;

namespace ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Queries
{
    public class GetDoorsQuery : IRequest<BaseResponse<PaginatedParameter<DoorDto>>>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }

    public class GetDoorQueryHandler(IUnitOfWorkRepository _unitOfWorkRepository) : IRequestHandler<GetDoorsQuery, BaseResponse<PaginatedParameter<DoorDto>>>
    {
        public async Task<BaseResponse<PaginatedParameter<DoorDto>>> Handle(GetDoorsQuery request, CancellationToken cancellationToken)
        {
            var doors = _unitOfWorkRepository.DoorRepository
                .GetAllQuery();

            var doorDto = doors.Select(x => new DoorDto
            {
                DateCreated = x.DateCreated,
                Location = x.Location,
                Name = x.Location,
                Id = x.Id,

            });

            PaginatedParameter<DoorDto> doorsResult = new(doorDto, request.Page, request.PageSize);
            return BaseResponse<PaginatedParameter<DoorDto>>.PassedResponse(Constants.ApiOkMessage, doorsResult);
        }
    }
}