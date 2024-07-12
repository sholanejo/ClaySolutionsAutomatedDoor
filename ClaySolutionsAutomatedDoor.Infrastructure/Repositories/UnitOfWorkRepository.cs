using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Repositories
{
    public class UnitOfWorkRepository(AutomatedDoorDbContext context) : IUnitOfWorkRepository
    {
        public IDoorPermissionRepository DoorPermissionRepository => new DoorPermissionRepository(context);

        public IAuditTrailRepository AuditTrailRepository => new AuditTrailRepository(context);

        public IDoorAccessControlGroupRepository DoorAccessControlGroupRepository => new DoorAccessControlGroupRepository(context);

        public IDoorRepository DoorRepository => new DoorRepository(context);

        public async Task<bool> CommitAsync()
        {
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}