using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Domain.Configurations;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Infrastructure.Persistence;
using ClaySolutionsAutomatedDoor.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ClaySolutionsAutomatedDoor.Infrastructure.Extensions
{
    public static class InfrastructureServicesExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
               .AddEntityFrameworkStores<AutomatedDoorDbContext>()
               .AddDefaultTokenProviders();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 38));
            services.AddDbContext<AutomatedDoorDbContext>(options =>
                options.UseMySql(connectionString, serverVersion));

            services.AddScoped<IDoorRepository, DoorRepository>();
            services.AddScoped<IAuditTrailRepository, AuditTrailRepository>();
            services.AddScoped<IDoorAccessControlGroupRepository, DoorAccessControlGroupRepository>();
            services.AddScoped<IDoorPermissionRepository, DoorPermissionRepository>();
            services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();

            services.Configure<BearerTokenConfiguration>(configuration.GetSection(BearerTokenConfiguration.SectionName));

            var bearerTokenConfig = configuration.GetSection(BearerTokenConfiguration.SectionName).Get<BearerTokenConfiguration>();
            services.AddAuthentication(authenticationOptions =>
            {
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(bearerTokenConfig.Key);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = bearerTokenConfig.Issuer,
                    ValidAudience = bearerTokenConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.NameIdentifier,
                };
            });
            return services;
        }
    }
}
