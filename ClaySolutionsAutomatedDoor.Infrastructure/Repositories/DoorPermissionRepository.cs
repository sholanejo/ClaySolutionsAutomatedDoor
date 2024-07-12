using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Repositories
{
    public class DoorPermissionRepository : BaseRepository<DoorPermission>, IDoorPermissionRepository
    {
        public DoorPermissionRepository(AutomatedDoorDbContext context) : base(context) { }
    }
}
