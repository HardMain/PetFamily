using Core.Validation;
using FluentValidation;
using SharedKernel.Constants;
using SharedKernel.Failures;

namespace Species.Application.Commands.AddBreed
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
