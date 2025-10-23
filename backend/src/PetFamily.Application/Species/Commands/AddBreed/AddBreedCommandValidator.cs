using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.Species.Commands.AddBreed
{
    public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
    {
        public AddBreedCommandValidator()
        {
            RuleFor(b => b.SpeciesId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("speciesId"));

            RuleFor(b => b.Request.Name)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("name"))
                .MaximumLength(Constants.MAX_LOW_TEXT_LENGTH)
                .WithError(Errors.General.ValueIsInvalid("name"));
        }
    }
}
