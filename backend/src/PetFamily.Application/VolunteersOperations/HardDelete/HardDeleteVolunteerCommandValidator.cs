using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.VolunteersOperations.HardDelete
{
    public class HardDeleteVolunteerCommandValidator : AbstractValidator<HardDeleteVolunteerCommand>
    {
        public HardDeleteVolunteerCommandValidator()
        {
            RuleFor(v => v.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
        }
    }
}