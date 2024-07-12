using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Repositories
{
    public class AuditTrailRepository : BaseRepository<AuditTrail>, IAuditTrailRepository
    {
        public AuditTrailRepository(AutomatedDoorDbContext context) : base(context)
        {
        }
    }
}
