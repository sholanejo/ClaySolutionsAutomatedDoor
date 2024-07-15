namespace ClaySolutionsAutomatedDoor.Application.Common.Models
{
    public class Constants
    {
        public const string UserAlreadyExistsMessage = "User already exist. Please proceed to login";
        public const string DoorAccessControlGroupNotFoundMessage = "The access control group specified was not found";
        public const string NewUserAddedMessage = "New User has been added with email address {0}";
        public const string ApiOkMessage = "Success";
        public const string FailedLoginAttemptMessage = "Your email address or password is invalid";
        public const string InActiveUserLoginAttemptMessage = "Your account is currently inactive, please contact admin for support";
        public const string UserLockedOutMessage = "Your account has been locked due to multiple failed login attempts. Please try again after 1 hour.";
        public const string UserBlockedOutMessage = "Your account has been locked due to multiple failed login attempts. Please contact support.";
        public const string LoginSuccessfulMessage = "Login Successful";
        public const string AddDoorAccessGroupFailureMessage = "Door access control group could not be added because a matching name was found";
        public const string DoorAccessGroupActivityMessage = "New door access control group with name {0} has been added";
        public const string AddDoorMessage = "New door with name {0} has been added";
        public const string DoorAlreadyExistMessage = "Door with name {0} already exists";
        public const string DoorDoesNotExistMessage = "The Door specified was not found";
        public const string DoorAlreadyAddedToGroupExistMessage = "The Door is already added to specified group";
        public const string DoorAddedToGroupMessage = "The Door is successfully added to {0} group";
        public const string UserDoesNotExistMessage = "The User was not found";
        public const string DeactivateUserMessage = "The User was successfully deactivated";
        public const string DeactivateUserFailedMessage = "Cannot deactivate an already inactive User";
        public const string ActivateUserFailedMessage = "The user is active";
        public const string ActivateUserMessage = "The User was successfully Activated";
        public const string AccessGroupDeletedMessage = "Door with Id {0} has been removed from {1}";
        public const string NoMatchingMessage = "No matching record found";
        public const string UpdateAccessControlGroup = "User already belongs to the specified access control group";
        public const string AttemptToOpenUnassignedDoor = "User attempted opening door not assigned";
        public const string AttemptToCloseUnassignedDoor = "User attempted closing door not assigned";
        public const string OpenDoorSuccessMessage = "User Successfully opened door {0}";
        public const string CloseDoorSuccessMessage = "User Successfully closed door {0}";
        public const string RoleNotFOundMessage = "Role was not found";
    }
}
