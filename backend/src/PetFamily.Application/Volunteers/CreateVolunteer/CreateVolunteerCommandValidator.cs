using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Contracts.Requests.Volunteers;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerRequest>
    {
        public CreateVolunteerCommandValidator()
        {
            RuleFor(v => v.FullName).MustBeValueObjects(fn => FullName.Create(fn.firstName, fn.lastName, fn.middleName));
            RuleFor(v => v.Email).MustBeValueObjects(Email.Create);
            RuleFor(v => v.PhoneNumber).MustBeValueObjects(PhoneNumber.Create);

            RuleFor(v => v.Description)
                .MaximumLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .WithError();

            RuleFor(v => v.ExperienceYears)
                .GreaterThanOrEqualTo(0)
                .WithError()
                .LessThanOrEqualTo(100)
                .WithError();
        }
    }
}