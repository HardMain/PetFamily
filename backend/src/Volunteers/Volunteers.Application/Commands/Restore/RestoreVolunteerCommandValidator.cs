using FluentValidation;
using Framework.Validation;
using SharedKernel.Failures;

namespace Volunteers.Application.Commands.Restore
{
    public class RestoreVolunteerCommandValidator : AbstractValidator<RestoreVolunteerCommand>
    {
        public RestoreVolunteerCommandValidator()
        {
            RuleFor(v => v.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());
        }
    }
}
