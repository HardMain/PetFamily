using FluentValidation;
using Framework.Validation;
using SharedKernel.Failures;
using Volunteers.Domain.ValueObjects;

namespace Volunteers.Application.Commands.DeletePetFiles
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