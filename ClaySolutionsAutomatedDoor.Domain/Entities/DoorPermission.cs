using System.ComponentModel.DataAnnotations.Schema;

namespace ClaySolutionsAutomatedDoor.Domain.Entities
{
    /// <summary>
    /// Represents the assignment of a door to a permission group.
    /// </summary>
    public class DoorPermission : BaseEntity
    {
        /// <summary>
        /// Represents the Permission group id
        /// </summary>
        [Column("door_access_control_group_id")]
        public Guid DoorAccessControlGroupId { get; set; }

        /// <summary>
        /// Represents the door id assigned to the permission group.
        /// </summary>
        [Column("door_id")]
        public Guid DoorId { get; set; }

        /// <summary>
        /// Navigational property linking to the PermissionGroup that has access to this door.
        /// </summary>
        public DoorAccessControlGroup DoorAccessControlGroup { get; set; }
    }
}
