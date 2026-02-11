using Core.Validation;
using FluentValidation;
using SharedKernel.Failures;

namespace Volunteers.Application.Commands.UpdatePetSupportStatus
{
    public class UpdatePetSupportStatusCommandValidator : AbstractValidator<UpdatePetSupportStatusCommand>
    {
        public UpdatePetSupportStatusCommandValidator()
        {
            RuleFor(v => v.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("volunteerId"));

            RuleFor(v => v.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("petId"));

            RuleFor(v => v.Request.SupportStatus)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("supportStatus"));
        }
    }
}