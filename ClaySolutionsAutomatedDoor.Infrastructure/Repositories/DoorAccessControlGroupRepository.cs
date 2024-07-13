using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Repositories
{
    public class DoorAccessControlGroupRepository : BaseRepository<DoorAccessControlGroup>, IDoorAccessControlGroupRepository
    {
        private readonly AutomatedDoorDbContext _dbContext;
        public DoorAccessControlGroupRepository(AutomatedDoorDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<DoorAccessControlGroup> GetDoorAccessControlGroupByName(string groupName)
        {
            return await _dbContext.DoorAccessControlGroup
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.GroupName.ToLower().Equals(groupName.ToLower()));

        }
    }
}
