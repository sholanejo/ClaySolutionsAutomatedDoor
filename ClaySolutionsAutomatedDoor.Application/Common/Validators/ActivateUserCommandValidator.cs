using ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands;
using FluentValidation;

namespace ClaySolutionsAutomatedDoor.Application.Common.Validators
{
    public class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
    {
        public ActivateUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id Cannot be empty");
        }
    }
}
