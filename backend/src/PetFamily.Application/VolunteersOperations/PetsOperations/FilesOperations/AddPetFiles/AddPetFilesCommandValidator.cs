using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.VolunteersOperations.PetsOperations.FilesOperations.AddPetFiles
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
                .MustBeValueObjects(f => FilePath.Create(f.FileName));
        }
    }
}