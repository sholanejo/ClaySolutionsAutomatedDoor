using ClaySolutionsAutomatedDoor.Application.Features.DoorAccessControlFeatures.Commands;
using FluentValidation;

namespace ClaySolutionsAutomatedDoor.Application.Common.Validators
{
    public class AddDoorAccessControlGroupValidator : AbstractValidator<AddDoorAccessControlGroupCommand>
    {
        public AddDoorAccessControlGroupValidator()
        {
            RuleFor(x => x.GroupName).NotEmpty().WithMessage("Group Name cannot be empty");
        }
    }


}