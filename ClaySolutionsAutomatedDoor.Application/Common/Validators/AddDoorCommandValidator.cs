using ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Commands;
using FluentValidation;

namespace ClaySolutionsAutomatedDoor.Application.Common.Validators
{
    public class AddDoorCommandValidator : AbstractValidator<AddDoorCommand>
    {
        public AddDoorCommandValidator()
        {
            RuleFor(x => x.DoorName).NotEmpty().WithMessage("Door name cannot be empty");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Door Location cannot be empty");
        }
    }
}
