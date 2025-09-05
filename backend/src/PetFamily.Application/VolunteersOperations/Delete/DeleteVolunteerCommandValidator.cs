using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.VolunteersOperations.Delete
{
    public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
    {
        public DeleteVolunteerCommandValidator()
        {
            RuleFor(v => v.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("volunteerId"));
        }
    }
}