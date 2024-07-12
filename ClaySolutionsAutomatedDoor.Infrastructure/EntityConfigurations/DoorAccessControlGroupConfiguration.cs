using ClaySolutionsAutomatedDoor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaySolutionsAutomatedDoor.Infrastructure.EntityConfigurations
{
    public class DoorAccessControlGroupConfiguration : IEntityTypeConfiguration<DoorAccessControlGroup>
    {
        public void Configure(EntityTypeBuilder<DoorAccessControlGroup> builder)
        {
            builder.HasKey(dacg => dacg.Id);

            builder.HasMany(dacg => dacg.Users)
                   .WithOne(u => u.DoorAccessControlGroup)
                   .HasForeignKey(u => u.DoorAccessControlGroupId);

            builder.HasMany(dacg => dacg.DoorPermission)
                   .WithOne(dp => dp.DoorAccessControlGroup)
                   .HasForeignKey(dp => dp.DoorAccessControlGroupId);
        }
    }
}
