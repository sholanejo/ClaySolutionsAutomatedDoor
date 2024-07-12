namespace ClaySolutionsAutomatedDoor.Application.Common.Repositories
{
    public interface IUnitOfWorkRepository
    {
        public IDoorPermissionRepository DoorPermissionRepository { get; }

        public IAuditTrailRepository AuditTrailRepository { get; }

        public IDoorAccessControlGroupRepository DoorAccessControlGroupRepository { get; }

        public IDoorRepository DoorRepository { get; }

        Task<bool> CommitAsync();
    }
}
