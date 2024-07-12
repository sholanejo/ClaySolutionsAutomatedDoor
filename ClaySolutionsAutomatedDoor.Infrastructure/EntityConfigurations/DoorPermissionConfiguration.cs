using ClaySolutionsAutomatedDoor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaySolutionsAutomatedDoor.Infrastructure.EntityConfigurations
{
    public class DoorPermissionConfiguration : IEntityTypeConfiguration<DoorPermission>
    {
        public void Configure(EntityTypeBuilder<DoorPermission> builder)
        {
            builder.HasKey(dp => dp.Id);

            builder.HasOne(dp => dp.DoorAccessControlGroup)
                   .WithMany(dacg => dacg.DoorPermission)
                   .HasForeignKey(dp => dp.DoorAccessControlGroupId);

            builder.HasOne<Door>()
                   .WithMany()
                   .HasForeignKey(dp => dp.DoorId);
        }
    }
}
