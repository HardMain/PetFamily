using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Create
{
    public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
    {
        public CreateVolunteerCommandValidator()
        {
            RuleFor(v => v.Request.FullName).MustBeValueObjects(fn => FullName.Create(fn.firstName, fn.lastName, fn.middleName));
            RuleFor(v => v.Request.Email).MustBeValueObjects(Email.Create);
            RuleFor(v => v.Request.PhoneNumber).MustBeValueObjects(PhoneNumber.Create);

            RuleFor(v => v.Request.Description)
                .MaximumLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .WithError(Errors.General.ValueIsInvalid("description"));

            RuleFor(v => v.Request.ExperienceYears)
                .GreaterThanOrEqualTo(0)
                .WithError(Errors.General.ValueIsInvalid("experienceYears"))
                .LessThanOrEqualTo(100)
                .WithError(Errors.General.ValueIsInvalid("experienceYears"));

            RuleForEach(v => v.Request.SocialNetworks).MustBeValueObjects(sn => SocialNetwork.Create(sn.URL, sn.Platform));
            RuleForEach(v => v.Request.DonationsInfo).MustBeValueObjects(di => DonationInfo.Create(di.Title, di.Description));
        }
    }
}