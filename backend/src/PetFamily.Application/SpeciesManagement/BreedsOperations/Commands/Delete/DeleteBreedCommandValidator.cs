using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.SpeciesManagement.BreedsOperations.Commands.Delete
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
