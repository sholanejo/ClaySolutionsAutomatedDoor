using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Repositories
{
    public class DoorRepository : BaseRepository<Door>, IDoorRepository
    {
        public DoorRepository(AutomatedDoorDbContext context) : base(context) { }
    }
}
