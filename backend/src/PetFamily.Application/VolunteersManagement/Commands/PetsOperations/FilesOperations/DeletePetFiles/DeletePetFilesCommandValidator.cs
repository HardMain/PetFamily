using FluentValidation;
using FluentValidation.AspNetCore;
using PetFamily.Application.Validation;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.VolunteersManagement.Commands.PetsOperations.FilesOperations.DeletePetFiles
{
    public class DeletePetFilesCommandValidator : AbstractValidator<DeletePetFilesCommand>
    {
        public DeletePetFilesCommandValidator()
        {
            RuleFor(pf => pf.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(pf => pf.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(pf => pf.Request.ObjectNameList)
                .MustBeValueObjects(FilePath.Create);
        }
    }
}