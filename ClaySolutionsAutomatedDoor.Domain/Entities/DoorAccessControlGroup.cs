using System.ComponentModel.DataAnnotations.Schema;

namespace ClaySolutionsAutomatedDoor.Domain.Entities
{
    public class DoorAccessControlGroup : BaseEntity
    {
        [Column("group_name")]
        public string GroupName { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<DoorPermission> DoorPermission { get; set; }
    }
}
