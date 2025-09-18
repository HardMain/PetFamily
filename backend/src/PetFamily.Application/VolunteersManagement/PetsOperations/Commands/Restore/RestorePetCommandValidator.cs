using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Restore
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
