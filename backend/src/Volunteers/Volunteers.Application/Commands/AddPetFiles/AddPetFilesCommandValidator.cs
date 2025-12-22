using FluentValidation;
using Framework.Validation;
using SharedKernel.Constants;
using SharedKernel.Failures;
using Volunteers.Domain.ValueObjects;

namespace Volunteers.Application.Commands.AddPetFiles
{
    public class AddPetFilesCommandValidator : AbstractValidator<AddPetFilesCommand>
    {
        public AddPetFilesCommandValidator()
        {
            RuleFor(pf => pf.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("volunteerId"));

            RuleFor(pf => pf.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("petId"));

            RuleForEach(pf => pf.Files)
                .Must(f => f.Content != null && f.Content.CanRead)
                .WithError(Errors.General.ValueIsInvalid("petFiles"));

            RuleForEach(pf => pf.Files)
                .Must(f => f.Content.Length > 0)
                .WithError(Errors.PetFile.FileIsEmpty());

            RuleForEach(pf => pf.Files)
                .Must(f => f.Content.Length <= Constants.MAX_FILE_SIZE)
                .WithError(Errors.PetFile.FileTooLarge());

            RuleForEach(pf => pf.Files)
                .MustBeValueObjects(f => FilePath.Create(f.FileName));
        }
    }
}