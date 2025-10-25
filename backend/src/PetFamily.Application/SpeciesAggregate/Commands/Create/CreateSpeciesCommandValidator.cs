using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.SpeciesAggregate.Commands.Create
{
    public class CreateSpeciesCommandValidator : AbstractValidator<CreateSpeciesCommand>
    {
        public CreateSpeciesCommandValidator()
        {
            RuleFor(b => b.Request.Name)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("name"))
                .MaximumLength(Constants.MAX_LOW_TEXT_LENGTH)
                .WithError(Errors.General.ValueIsInvalid("name"));
        }
    }
}
