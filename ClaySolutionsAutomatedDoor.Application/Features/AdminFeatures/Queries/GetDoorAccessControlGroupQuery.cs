using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Common.Utility;
using ClaySolutionsAutomatedDoor.Domain.Dtos;
using MediatR;

namespace ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Query
{
    public class GetDoorAccessControlGroupQuery : IRequest<BaseResponse<PaginatedParameter<DoorAccessControlGroupDto>>>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }

    public class GetDoorAccessControlGroupQueryHandler(IUnitOfWorkRepository _unitOfWorkRepository
        ) : IRequestHandler<GetDoorAccessControlGroupQuery, BaseResponse<PaginatedParameter<DoorAccessControlGroupDto>>>
    {
        public async Task<BaseResponse<PaginatedParameter<DoorAccessControlGroupDto>>> Handle(GetDoorAccessControlGroupQuery request, CancellationToken cancellationToken)
        {
            var doorDccessControlGroup = _unitOfWorkRepository.DoorAccessControlGroupRepository
                .GetAllQuery();

            var doorAccessControlGroupDto = doorDccessControlGroup.Select(x => new DoorAccessControlGroupDto
            {
                DoorAccessControlGroupId = x.Id,
                DoorPermission = x.DoorPermission.Select(d => new DoorPermissionDto
                {
                    DoorId = d.DoorId,
                    DoorAccessControlGroupId = d.DoorAccessControlGroupId,
                    DateCreated = d.DateCreated,

                }).ToList(),
                GroupName = x.GroupName,
                Users = x.Users.Select(u => new ApplicationUserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,

                }).ToList(),
            });

            PaginatedParameter<DoorAccessControlGroupDto> doorAccessControlGroupList = new(doorAccessControlGroupDto, request.Page, request.PageSize);
            return BaseResponse<PaginatedParameter<DoorAccessControlGroupDto>>.PassedResponse(Constants.ApiOkMessage, doorAccessControlGroupList);
        }
    }
}
