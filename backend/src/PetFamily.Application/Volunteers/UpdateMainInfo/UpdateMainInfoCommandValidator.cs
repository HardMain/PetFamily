using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateMainInfo
{
    public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
    {
        public UpdateMainInfoCommandValidator()
        {
            RuleFor(i => i.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());

            RuleFor(i => i.Request.Email).MustBeValueObjects(Email.Create);
            RuleFor(i => i.Request.PhoneNumber).MustBeValueObjects(PhoneNumber.Create);
            RuleFor(i => i.Request.FullName).MustBeValueObjects(
                fn => FullName.Create(fn.firstName, fn.lastName, fn.middleName));

            RuleFor(i => i.Request.Description)
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
