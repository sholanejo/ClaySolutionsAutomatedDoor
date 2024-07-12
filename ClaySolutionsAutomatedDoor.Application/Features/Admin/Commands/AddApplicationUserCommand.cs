using ClaySolutionsAutomatedDoor.Application.Common.Models;
using ClaySolutionsAutomatedDoor.Application.Common.Repositories;
using ClaySolutionsAutomatedDoor.Application.Common.Utility;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using ClaySolutionsAutomatedDoor.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Transactions;


namespace ClaySolutionsAutomatedDoor.Application.Features.Admin.Commands
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

        public AddApplicationUserCommandHandler(UserManager<ApplicationUser> userManager,
            IUnitOfWorkRepository unitOfWorkRepository)
        {
            _userManager = userManager;
            _unitOfWorkRepository = unitOfWorkRepository;
        }


        public async Task<BaseResponse> Handle(AddApplicationUserCommand request, CancellationToken cancellationToken)
        {
            if (await DoesUserExist(request.Email) is false)
            {
                return BaseResponse.FailedResponse(Constants.UserAlreadyExistsMessage, StatusCodes.Status409Conflict);
            }

            var doorControlAccessGroup = await _unitOfWorkRepository.DoorAccessControlGroupRepository.GetByIdAsync(request.DoorAccessControlGroupId);
            if (doorControlAccessGroup is null)
            {
                return BaseResponse.FailedResponse(Constants.DoorAccessGroupNotFoundMessage, StatusCodes.Status400BadRequest);
            }

            var newUser = BuildAppUser(request);
            var notes = string.Format(Constants.NewUserAddedMessage, request.Email);
            var auditTrail = BuildAuditTrail(request.CreatedBy, notes);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                scope.Dispose();
                return BaseResponse.FailedResponse(ValidationResult.GetIdentityResultErrors(result), StatusCodes.Status500InternalServerError);
            }

            await _userManager.AddToRoleAsync(newUser, Roles.RegularUser.ToString());
            await _unitOfWorkRepository.AuditTrailRepository.InsertAsync(auditTrail);
            await _unitOfWorkRepository.CommitAsync();

            scope.Complete();

            return BaseResponse.PassedResponse(Constants.Api200OkMessage, StatusCodes.Status201Created);
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
                ActorId = createdBy,
                IsSuccessful = true,
                Notes = notes,
            };
        }
    }
}