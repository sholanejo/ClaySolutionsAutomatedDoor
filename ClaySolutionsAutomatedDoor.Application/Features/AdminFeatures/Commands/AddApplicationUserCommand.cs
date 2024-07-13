using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Common.Utility;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Transactions;

namespace ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands
{
    public class AddApplicationUserCommand : IRequest<BaseResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid DoorAccessControlGroupId { get; set; }
        public string CreatedBy { get; set; }
    }

    public class AddApplicationUserCommandHandler : IRequestHandler<AddApplicationUserCommand, BaseResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly ILogger<AddApplicationUserCommandHandler> _logger;

        public AddApplicationUserCommandHandler(UserManager<ApplicationUser> userManager,
            IUnitOfWorkRepository unitOfWorkRepository,
            ILogger<AddApplicationUserCommandHandler> logger)
        {
            _userManager = userManager;
            _unitOfWorkRepository = unitOfWorkRepository;
            _logger = logger;
        }


        public async Task<BaseResponse> Handle(AddApplicationUserCommand request, CancellationToken cancellationToken)
        {
            request.Email = request.Email.Trim();
            if (await DoesUserExist(request.Email) is false)
            {
                _logger.LogWarning("User with email {0} cannot be added as the user already exists", request.Email);
                return BaseResponse.FailedResponse(Constants.UserAlreadyExistsMessage, StatusCodes.Status409Conflict);
            }

            var doorControlAccessGroup = await _unitOfWorkRepository.DoorAccessControlGroupRepository.GetByIdAsync(request.DoorAccessControlGroupId);
            if (doorControlAccessGroup is null)
            {
                _logger.LogWarning("Door access control group with id {0} was not found", request.DoorAccessControlGroupId);
                return BaseResponse.FailedResponse(Constants.DoorAccessGroupNotFoundMessage, StatusCodes.Status400BadRequest);
            }

            _logger.LogInformation("Building object properties for adding new user...");
            var newUser = BuildAppUser(request);
            var notes = string.Format(Constants.NewUserAddedMessage, request.Email);
            var auditTrail = BuildAuditTrail(request.CreatedBy, notes);
            _logger.LogInformation("Building object properties for adding new user completed");

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("Unable to create new user with email {0} due to : {1}", request.Email, JsonConvert.SerializeObject(result.Errors));
                scope.Dispose();
                return BaseResponse.FailedResponse(ValidationResult.GetIdentityResultErrors(result), StatusCodes.Status500InternalServerError);
            }

            await _userManager.AddToRoleAsync(newUser, Roles.RegularUser.ToString());
            await _unitOfWorkRepository.AuditTrailRepository.InsertAsync(auditTrail);
            await _unitOfWorkRepository.CommitAsync();

            scope.Complete();

            _logger.LogInformation("User with email {0} was successfully created", request.Email);
            return BaseResponse.PassedResponse(Constants.ApiOkMessage, StatusCodes.Status201Created);
        }

        private async Task<bool> DoesUserExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null;
        }

        private ApplicationUser BuildAppUser(AddApplicationUserCommand request)
        {
            return new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = request.CreatedBy,
                DoorAccessControlGroupId = request.DoorAccessControlGroupId
            };
        }

        private AuditTrail BuildAuditTrail(string createdBy, string notes)
        {
            return new AuditTrail
            {
                DateCreated = DateTime.Now,
                PerformedBy = createdBy,
                Notes = notes,
            };
        }
    }
}