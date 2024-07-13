using ClaySolutionsAutomatedDoor.Domain.Entities;

namespace ClaySolutionsAutomatedDoor.Application.Common.Repositories
{
    public interface IDoorAccessControlGroupRepository : IBaseRepository<DoorAccessControlGroup>
    {
        Task<DoorAccessControlGroup> GetDoorAccessControlGroupByName(string groupName);

    }
}
