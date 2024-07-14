using ClaySolutionsAutomatedDoor.Application.Features.DoorFeatures.Queries;
using FluentValidation;

namespace ClaySolutionsAutomatedDoor.Application.Common.Validators
{
    public class GetSingleDoorQueryValidator : AbstractValidator<GetSingleDoorQuery>
    {
        public GetSingleDoorQueryValidator()
        {
            RuleFor(x => x.DoorId).NotEmpty().WithMessage("Door id cannot be empty");
        }
    }
}
