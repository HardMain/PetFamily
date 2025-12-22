using FluentValidation;
using Framework.Validation;
using SharedKernel.Failures;

namespace Volunteers.Application.Commands.DeletePet
{
    public class DeletePetCommandValidator : AbstractValidator<DeletePetCommand>
    {
        public DeletePetCommandValidator()
        {
            RuleFor(p => p.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("volunteerId"));

            RuleFor(p => p.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("petId"));
        }
    }
}