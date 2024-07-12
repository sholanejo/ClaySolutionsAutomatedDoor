using ClaySolutionsAutomatedDoor.Application.Features.Admin.Commands;
using ClaySolutionsAutomatedDoor.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ClaySolutionsAutomatedDoor.Application.Common.Validators
{
    public class AddApplicationUserCommandValidator : AbstractValidator<AddApplicationUserCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AddApplicationUserCommandValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MustAsync(BeAValidPassword)
                .MinimumLength(8).WithMessage("Password does not match password requirement policy.");

            RuleFor(x => x.DoorAccessControlGroupId)
                .NotEmpty().WithMessage("Door Access group ID is required.");
        }

        private async Task<bool> BeAValidPassword(string password, CancellationToken cancellationToken)
        {
            var dummyUser = new ApplicationUser();
            var result = await _userManager.PasswordValidators[0].ValidateAsync(_userManager, dummyUser, password);
            return result.Succeeded;
        }
    }
}
