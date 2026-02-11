using Core.Validation;
using FluentValidation;
using SharedKernel.Constants;
using SharedKernel.Failures;

namespace Species.Application.Commands.Create
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
