using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Repositories
{
    public class UnitOfWorkRepository(AutomatedDoorDbContext context) : IUnitOfWorkRepository
    {
        private IDoorPermissionRepository? _doorPermissionRepository;
        private IAuditTrailRepository? _auditTrailRepository;
        private IDoorAccessControlGroupRepository? _doorAccessControlGroupRepository;
        private IDoorRepository? _doorRepository;

        public IDoorPermissionRepository DoorPermissionRepository =>
            _doorPermissionRepository ??= new DoorPermissionRepository(context);

        public IAuditTrailRepository AuditTrailRepository =>
            _auditTrailRepository ??= new AuditTrailRepository(context);

        public IDoorAccessControlGroupRepository DoorAccessControlGroupRepository =>
            _doorAccessControlGroupRepository ??= new DoorAccessControlGroupRepository(context);

        public IDoorRepository DoorRepository =>
            _doorRepository ??= new DoorRepository(context);

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
        {
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}