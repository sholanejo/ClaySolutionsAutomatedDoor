using ClaySolutionsAutomatedDoor.Application.Features.AccountFeatures.Commands;
using FluentValidation;

namespace ClaySolutionsAutomatedDoor.Application.Common.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("Email address cannot be empty")
                .EmailAddress().WithMessage("Please provide a valid email address");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty")
                ;
        }
    }
}
