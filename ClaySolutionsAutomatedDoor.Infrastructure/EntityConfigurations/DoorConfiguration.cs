using ClaySolutionsAutomatedDoor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaySolutionsAutomatedDoor.Infrastructure.EntityConfigurations
{
    public class DoorConfiguration : IEntityTypeConfiguration<Door>
    {
        public void Configure(EntityTypeBuilder<Door> builder)
        {
            builder.HasKey(d => d.Id);
        }
    }
}
