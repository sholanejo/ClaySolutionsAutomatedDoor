using System.ComponentModel.DataAnnotations.Schema;

namespace ClaySolutionsAutomatedDoor.Domain.Entities
{
    /// <summary>
    /// Represents a log of activities related to door access and actions performed in the app
    /// </summary>
    public class AuditTrail
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user involved in creating the activity.
        /// </summary>
        [Column("performed_by")]
        public string PerformedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time the entity was created.
        /// </summary>
        [Column("date_created")]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Represents the navigational property of the user performing the action
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Description of the action that was performed
        /// </summary>
        [Column("notes")]
        public string Notes { get; set; }
    }
}
