using Microsoft.AspNetCore.Identity;

namespace ClaySolutionsAutomatedDoor.Application.Common.Utility
{
    internal class ValidationResult
    {
        public static string GetIdentityResultErrors(IdentityResult result)
        {
            return result.Errors.Aggregate(string.Empty, (current, err) => current + err.Description + "\n");
        }
    }
}
