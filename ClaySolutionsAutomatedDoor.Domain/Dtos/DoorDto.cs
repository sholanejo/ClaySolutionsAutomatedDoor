namespace ClaySolutionsAutomatedDoor.Domain.Dtos
{
    public class DoorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
