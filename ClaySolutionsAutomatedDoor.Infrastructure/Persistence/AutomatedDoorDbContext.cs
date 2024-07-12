using ClaySolutionsAutomatedDoor.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Persistence
{
    public class AutomatedDoorDbContext : IdentityDbContext<ApplicationUser>
    {
        public AutomatedDoorDbContext(DbContextOptions<AutomatedDoorDbContext> options) : base(options)
        {

        }

        public DbSet<Door> Door { get; set; }
        public DbSet<AuditTrail> AuditTrail { get; set; }
        public DbSet<DoorAccessControlGroup> DoorAccessControlGroup { get; set; }
        public DbSet<DoorPermission> DoorPermission { get; set; }

    }
}
