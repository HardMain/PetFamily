using Core.Validation;
using FluentValidation;
using SharedKernel.Constants;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;
using Volunteers.Domain.ValueObjects;

namespace Volunteers.Application.Commands.UpdateMainInfo
{
    public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
    {
        public UpdateMainInfoCommandValidator()
        {
            RuleFor(i => i.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(i => i.Request.Email).MustBeValueObjects(Email.Create);
            RuleFor(i => i.Request.PhoneNumber).MustBeValueObjects(PhoneNumber.Create);
            RuleFor(i => i.Request.FullName).MustBeValueObjects(
                fn => FullName.Create(fn.FirstName, fn.LastName, fn.MiddleName));

            RuleFor(i => i.Request.Description)
                .NotEmpty()
                .MaximumLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .WithError(Errors.General.ValueIsInvalid("description"));

            RuleFor(i => i.Request.ExperienceYears)
                .GreaterThanOrEqualTo(0)
                .WithError(Errors.General.ValueIsInvalid("experienceYears"))
                .LessThanOrEqualTo(100)
                .WithError(Errors.General.ValueIsInvalid("experienceYears"));
        }
    }
}
