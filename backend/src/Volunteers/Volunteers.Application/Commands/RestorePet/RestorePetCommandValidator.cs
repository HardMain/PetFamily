using Core.Validation;
using FluentValidation;
using SharedKernel.Failures;

namespace Volunteers.Application.Commands.RestorePet
{
    public class RestorePetCommandValidator : AbstractValidator<RestorePetCommand>
    {
        public RestorePetCommandValidator()
        {
            RuleFor(v => v.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("volunteerId"));

            RuleFor(v => v.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("petId"));
        }
    }
}
