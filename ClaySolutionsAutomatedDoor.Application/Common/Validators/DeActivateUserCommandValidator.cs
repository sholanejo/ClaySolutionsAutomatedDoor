using ClaySolutionsAutomatedDoor.Application.Features.AdminFeatures.Commands;
using FluentValidation;

namespace ClaySolutionsAutomatedDoor.Application.Common.Validators
{
    public class DeActivateUserCommandValidator : AbstractValidator<DeActivateUserCommand>
    {
        public DeActivateUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id cannot be empty");
        }
    }
}
