using Microsoft.AspNetCore.Authorization;

namespace ClaySolutionsAutomatedDoor.API.AuthorizationRequirement
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
