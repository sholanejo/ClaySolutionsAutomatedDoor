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
        [Column("created_by")]
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time the entity was created.
        /// </summary>
        [Column("date_created")]
        public DateTime DateCreated { get; set; }


        /// <summary>
        /// Represents the userid of the actor performing the action
        /// </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// Represents the navigational property of the user performing the action
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Represents the door id of the door the action is being performed on
        /// </summary>
        [Column("door_id")]
        public Guid DoorId { get; set; }
        /// <summary>
        /// Represents the navigational property for the door of the actor performing the action
        /// </summary>
        public Door Door { get; set; }

        /// <summary>
        /// Represents if the action performed was successful i.e results in a 200 response
        /// </summary>
        [Column("is_successful")]
        public bool IsSuccessful { get; set; }
    }
}
