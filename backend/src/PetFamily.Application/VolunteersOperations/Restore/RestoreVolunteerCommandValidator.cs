using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.VolunteersOperations.Restore
{
    public class RestoreVolunteerCommandValidator : AbstractValidator<RestoreVolunteerCommand>
    {
        public RestoreVolunteerCommandValidator()
        {
            RuleFor(v => v.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
        }
    }
}
