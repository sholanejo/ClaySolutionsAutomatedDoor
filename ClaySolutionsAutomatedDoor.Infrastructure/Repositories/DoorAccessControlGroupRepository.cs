using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Repositories
{
    public class DoorAccessControlGroupRepository : BaseRepository<DoorAccessControlGroup>, IDoorAccessControlGroupRepository
    {
        public DoorAccessControlGroupRepository(AutomatedDoorDbContext context) : base(context)
        {
        }
    }
}
