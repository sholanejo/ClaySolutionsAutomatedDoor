namespace ClaySolutionsAutomatedDoor.Domain.Dtos
{
    public class DoorAccessControlGroupDto
    {
        public Guid DoorAccessControlGroupId { get; set; }
        public string GroupName { get; set; }
        public ICollection<ApplicationUserDto> Users { get; set; }
        public ICollection<DoorPermissionDto> DoorPermission { get; set; }
    }
}
