using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Domain.Configurations;
using ClaySolutionsAutomatedDoor.Domain.Dtos;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClaySolutionsAutomatedDoor.Application.Features.Account.Commands
{
    public class LoginCommand : IRequest<BaseResponse<LoginResponseDto>>
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler(ILogger<LoginCommandHandler> _logger,
        UserManager<ApplicationUser> _userManager,
        SignInManager<ApplicationUser> _signInManager,
        IOptions<BearerTokenConfiguration> _bearerTokenConfig,
        RoleManager<IdentityRole> _roleManager)
        : IRequestHandler<LoginCommand, BaseResponse<LoginResponseDto>>
    {
        public async Task<BaseResponse<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            request.EmailAddress = request.EmailAddress.Trim();
            var applicationUser = await _userManager.FindByEmailAsync(request.EmailAddress);
            if (applicationUser == null)
            {
                _logger.LogError("Invalid email {0} attempt to login", request.EmailAddress);
                return BaseResponse<LoginResponseDto>.FailedResponse(Constants.FailedLoginAttemptMessage, StatusCodes.Status400BadRequest);
            }

            if (!applicationUser.IsActive)
            {
                _logger.LogWarning("Inactive user {0} attempted sign in", request.EmailAddress);
                return BaseResponse<LoginResponseDto>.FailedResponse(Constants.InActiveUserLoginAttemptMessage, StatusCodes.Status403Forbidden);
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(applicationUser, request.Password, true);
            var lockedOut = applicationUser.AccessFailedCount;

            if (!signInResult.Succeeded)
            {
                _logger.LogError("Attempted User {0} sign in failed.", request.EmailAddress);
                lockedOut++;

                if (signInResult.IsLockedOut && lockedOut == 3)
                {
                    return BaseResponse<LoginResponseDto>.FailedResponse(Constants.UserLockedOutMessage, StatusCodes.Status423Locked);
                }

                if (signInResult.IsNotAllowed)
                {
                    return BaseResponse<LoginResponseDto>.FailedResponse(Constants.UserBlockedOutMessage, StatusCodes.Status423Locked);
                }

                return BaseResponse<LoginResponseDto>.FailedResponse(Constants.FailedLoginAttemptMessage, StatusCodes.Status400BadRequest);
            }

            var claims = await _GenerateUserClaims(applicationUser);
            var bearerToken = _GenerateBearerToken(claims);

            LoginResponseDto loginData = new() { AccessToken = bearerToken, UserId = applicationUser.Id, ExpiresInMinutes = _bearerTokenConfig.Value.ExpiryMinutes };

            return BaseResponse<LoginResponseDto>.PassedResponse(Constants.LoginSuccessfulMessage, loginData);
        }


        private async Task<List<Claim>> _GenerateUserClaims(ApplicationUser applicationUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, applicationUser.Id),
                new Claim(ClaimTypes.Email, applicationUser.Email)
            };

            var roles = await _userManager.GetRolesAsync(applicationUser);
            var permissions = new List<Claim>();

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                var identityRole = await _roleManager.FindByNameAsync(role);
                if (identityRole != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(identityRole);
                    permissions.AddRange(roleClaims.Where(x => x.Type == "Permission").ToList());
                }
            }

            claims.AddRange(permissions);
            return claims;
        }

        private string _GenerateBearerToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_bearerTokenConfig.Value.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_bearerTokenConfig.Value.ExpiryMinutes));

            var token = new JwtSecurityToken(
                issuer: _bearerTokenConfig.Value.Issuer,
                audience: _bearerTokenConfig.Value.Audience,
                claims: claims,
                expires: expiry,
               signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}