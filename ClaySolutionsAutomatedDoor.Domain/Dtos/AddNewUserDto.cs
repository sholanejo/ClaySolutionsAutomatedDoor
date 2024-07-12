namespace ClaySolutionsAutomatedDoor.Domain.Dtos
{
    public class AddNewUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid AccessGroupId { get; set; }
    }
}
