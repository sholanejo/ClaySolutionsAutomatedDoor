namespace ClaySolutionsAutomatedDoor.Domain.Dtos
{
    public class AuditTrailDto
    {
        public Guid UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Notes { get; set; }
    }
}
