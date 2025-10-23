using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.Volunteers.Commands.SetMainPhotoPet
{
    public class SetPetMainPhotoCommandValidator : AbstractValidator<SetPetMainPhotoCommand>
    {
        public SetPetMainPhotoCommandValidator()
        {
            RuleFor(v => v.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("volunteerId"));

            RuleFor(v => v.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("petId"));

            RuleFor(v => v.Request.Path)
                .MustBeValueObjects(FilePath.Create);
        }
    }
}
