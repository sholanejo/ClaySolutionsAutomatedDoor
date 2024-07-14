namespace ClaySolutionsAutomatedDoor.Domain.Dtos
{
    public class DoorPermissionDto
    {
        public Guid DoorId { get; set; }
        public Guid DoorAccessControlGroupId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
