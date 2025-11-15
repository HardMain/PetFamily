using FluentValidation;
using Framework.Validation;
using SharedKernel.Failures;

namespace Volunteers.Application.Commands.MovePet
{
    public class MovePetCommandValidator : AbstractValidator<MovePetCommand>
    {
        public MovePetCommandValidator()
        {
            RuleFor(p => p.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("volunteerId"));

            RuleFor(p => p.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("petId"));

            RuleFor(p => p.Request.newPosition)
                .GreaterThanOrEqualTo(1)
                .WithError(Errors.General.ValueIsInvalid("newPosition"));
        }
    }
}