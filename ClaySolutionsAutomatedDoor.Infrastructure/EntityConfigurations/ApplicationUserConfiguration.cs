using ClaySolutionsAutomatedDoor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaySolutionsAutomatedDoor.Infrastructure.EntityConfigurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(au => au.Id);

            builder.HasOne(au => au.DoorAccessControlGroup)
                .WithMany(dacg => dacg.Users)
                .HasForeignKey(au => au.DoorAccessControlGroupId);

            builder.HasMany(au => au.AuditTrail)
                .WithOne(at => at.User)
                .HasForeignKey(at => at.PerformedBy);
        }
    }
}