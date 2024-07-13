namespace ClaySolutionsAutomatedDoor.Application.Common.Models
{
    public class Constants
    {
        public const string UserAlreadyExistsMessage = "User already exist. Please proceed to login";
        public const string DoorAccessGroupNotFoundMessage = "The access group specified was not found";
        public const string NewUserAddedMessage = "New User has been added with email address {0}";
        public const string ApiOkMessage = "Success";
        public const string FailedLoginAttemptMessage = "Your email address or password is invalid";
        public const string InActiveUserLoginAttemptMessage = "Your account is currently inactive, please contact admin for support";
        public const string UserLockedOutMessage = "Your account has been locked due to multiple failed login attempts. Please try again after 1 hour.";
        public const string UserBlockedOutMessage = "Your account has been locked due to multiple failed login attempts. Please contact support.";
        public const string LoginSuccessfulMessage = "Login Successful";
        public const string AddDoorAccessGroupFailureMessage = "Door access control group could not be added because a matching name was found";
        public const string DoorAccessGroupActivityMessage = "New door access control group with name {0} has been added";
    }
}
