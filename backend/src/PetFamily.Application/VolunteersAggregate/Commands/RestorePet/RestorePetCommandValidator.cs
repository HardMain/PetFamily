using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.VolunteersAggregate.Commands.RestorePet
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
