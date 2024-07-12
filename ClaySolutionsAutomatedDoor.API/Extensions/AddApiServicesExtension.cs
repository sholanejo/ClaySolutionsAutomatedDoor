using ClaySolutionsAutomatedDoor.API.AuthorizationRequirement;
using Microsoft.AspNetCore.Authorization;

namespace ClaySolutionsAutomatedDoor.API.Extensions
{
    public static class AddApiServicesExtension
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddCors(options =>
            {
                options.AddPolicy("corsPolicy", corsOption =>
                {
                    corsOption.AllowAnyMethod();
                    corsOption.AllowAnyHeader();
                    corsOption.AllowCredentials();
                    corsOption.SetIsOriginAllowed(origin =>
                    {
                        if (string.IsNullOrWhiteSpace(origin)) return false;
                        if (origin.ToLower().StartsWith("http://localhost") && builder.Environment.IsDevelopment()) return true;
                        if (origin.ToLower().Contains("zedvance.com")) return true;
                        return false;
                    }); // allow any origin
                });
            });
            return services;
        }
    }
}