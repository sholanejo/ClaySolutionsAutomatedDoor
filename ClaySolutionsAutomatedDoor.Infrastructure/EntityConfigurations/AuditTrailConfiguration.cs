using ClaySolutionsAutomatedDoor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaySolutionsAutomatedDoor.Infrastructure.EntityConfigurations
{
    public class AuditTrailConfiguration : IEntityTypeConfiguration<AuditTrail>
    {
        public void Configure(EntityTypeBuilder<AuditTrail> builder)
        {
            builder.HasKey(at => at.Id);

            builder.HasOne(at => at.User)
                   .WithMany(u => u.AuditTrail)
                   .HasForeignKey(at => at.ActorId);
        }
    }
}
