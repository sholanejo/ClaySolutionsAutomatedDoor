using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaySolutionsAutomatedDoor.Domain.Entities
{
    /// <summary>
    /// Represents the base entity in which all other entities derive from because of it's common properties.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entities.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user involved in creating the entity.
        /// </summary>
        [Column("created_by")]
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time the entity was created.
        /// </summary>
        [Column("date_created")]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the date and time an entity was last modified.
        /// </summary>

        [Column("last_modified_date")]
        public DateTime LastModifiedDate { get; set; }
    }
}
