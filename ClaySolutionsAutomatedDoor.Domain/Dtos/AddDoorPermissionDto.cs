namespace ClaySolutionsAutomatedDoor.Domain.Dtos
{
    public class AddDoorPermissionDto
    {
        public Guid DoorId { get; set; }
        public Guid DoorAccessControlGroupId { get; set; }
    }
}
