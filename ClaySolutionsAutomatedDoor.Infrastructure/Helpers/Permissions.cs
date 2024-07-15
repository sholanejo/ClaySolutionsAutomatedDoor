namespace ClaySolutionsAutomatedDoor.Infrastructure.Helpers
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }

        public static class ApplicationUser
        {
            public const string Delete = "Permissions.User.Delete";
            public const string Create = "Permissions.User.Create";
        }
    }
}
