namespace ClaySolutionsAutomatedDoor.Domain.Entities
{
    public class DoorAccessControlGroup : BaseEntity
    {
        public string GroupName { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<DoorPermission> DoorPermission { get; set; }
    }
}
