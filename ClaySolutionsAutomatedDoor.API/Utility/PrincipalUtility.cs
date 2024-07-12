using System.Security.Claims;
using System.Security.Principal;

namespace ClaySolutionsAutomatedDoor.API.Utility
{
    public static class PrincipalUtility
    {
        public static string GetUserId(this IIdentity identity)
        {
            return GetClaimValue(identity, ClaimTypes.NameIdentifier);
        }

        private static string GetClaimValue(IIdentity identity, string claimType)
        {
            var claimIdentity = (ClaimsIdentity)identity;
            return claimIdentity.Claims.GetClaimValue(claimType);
        }

        private static string GetClaimValue(this IEnumerable<Claim> claims, string claimType)
        {

            var claimsList = new List<Claim>(claims);
            var claim = claimsList.Find(c => c.Type == claimType);
            return claim != null ? claim.Value : null;
        }
    }
}
