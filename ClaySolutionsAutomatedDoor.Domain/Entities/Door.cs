using System.ComponentModel.DataAnnotations.Schema;

namespace ClaySolutionsAutomatedDoor.Domain.Entities
{
    public class Door : BaseEntity
    {
        /// <summary>
        /// Represents the name of the door
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// Represents the location of the door
        /// </summary>
        [Column("location")]
        public string Location { get; set; }
    }
}
