using Microsoft.AspNetCore.Authorization;

namespace ClaySolutionsAutomatedDoor.API.AuthorizationRequirement
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionAuthorizationHandler() { }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }
            var permissionss = context.User.Claims.Where(x => x.Type == "Permission" &&
                                                                x.Value == requirement.Permission &&
                                                                x.Issuer == "https://localhost:7149");
            if (permissionss.Any())
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
