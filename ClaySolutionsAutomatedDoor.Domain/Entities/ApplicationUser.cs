using Microsoft.AspNetCore.Identity;

namespace ClaySolutionsAutomatedDoor.Domain.Entities
{
    /// <summary>
    /// Represents a user within the system.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// The user's first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// The user's last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        ///  Indicates who created the user record.
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the user is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The date when the user was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// The date when the user record was modified.
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// the id of the door access control group the user belongs to
        /// </summary>
        public Guid DoorAccessControlGroupId { get; set; }

        /// <summary>
        /// A collection of Audit Logs entries associated with the user.
        /// </summary>
        public ICollection<AuditTrail> AuditTrail { get; set; }

        /// <summary>
        /// Navigational property between the user and the door access control group they are in
        /// </summary>
        public DoorAccessControlGroup DoorAccessControlGroup { get; set; }
    }
}
