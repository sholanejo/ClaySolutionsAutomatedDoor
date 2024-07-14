namespace ClaySolutionsAutomatedDoor.Domain.Dtos
{
    public class RemoveDoorFromDoorPermissionDto
    {
        public required Guid DoorId { get; set; }
        public required Guid DoorAccessControlGroupId { get; set; }
    }
}
