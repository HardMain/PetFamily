using FluentValidation;
using FluentValidation.AspNetCore;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.VolunteersAggregate.ValueObjects;

namespace PetFamily.Application.VolunteersAggregate.Commands.DeletePetFiles
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