using Core.Validation;
using FluentValidation;
using SharedKernel.Failures;

namespace Species.Application.Commands.DeleteBreed
{
    public class DeleteBreedCommandValidator : AbstractValidator<DeleteBreedCommand>
    {
        public DeleteBreedCommandValidator()
        {
            RuleFor(b => b.SpeciesId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(b => b.BreedId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
        }
    }
}
