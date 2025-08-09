using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.Volunteers.SoftDelete
{
    public class SoftDeleteVolunteerCommandValidator : AbstractValidator<SoftDeleteVolunteerCommand>
    {
        public SoftDeleteVolunteerCommandValidator()
        {
            RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        }
    }
}