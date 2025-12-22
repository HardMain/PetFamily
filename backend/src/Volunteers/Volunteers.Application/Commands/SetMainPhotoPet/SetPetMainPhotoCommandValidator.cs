using FluentValidation;
using Framework.Validation;
using SharedKernel.Failures;
using Volunteers.Domain.ValueObjects;

namespace Volunteers.Application.Commands.SetMainPhotoPet
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
