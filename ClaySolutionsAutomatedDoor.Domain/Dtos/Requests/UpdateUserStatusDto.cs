using System.ComponentModel.DataAnnotations;

namespace ClaySolutionsAutomatedDoor.Domain.Dtos.Requests
{
    public class UpdateUserStatusDto
    {
        [Required]
        public bool IsActive { get; set; }
    }
}
