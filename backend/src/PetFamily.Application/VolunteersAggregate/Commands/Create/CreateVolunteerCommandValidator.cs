using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteersAggregate.ValueObjects;

namespace PetFamily.Application.VolunteersAggregate.Commands.Create
{
    public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
    {
        public CreateVolunteerCommandValidator()
        {
            RuleFor(v => v.Request.FullName)
                .MustBeValueObjects(fn => FullName.Create(fn.FirstName, fn.LastName, fn.MiddleName));
            RuleFor(v => v.Request.Email)
                .MustBeValueObjects(Email.Create);
            RuleFor(v => v.Request.PhoneNumber)
                .MustBeValueObjects(PhoneNumber.Create);

            RuleFor(v => v.Request.Description)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("description"))
                .MaximumLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .WithError(Errors.General.ValueIsInvalid("description"));

            RuleFor(v => v.Request.ExperienceYears)
                .GreaterThanOrEqualTo(0)
                .WithError(Errors.General.ValueIsInvalid("experienceYears"))
                .LessThanOrEqualTo(100)
                .WithError(Errors.General.ValueIsInvalid("experienceYears"));

            When(p => p.Request.SocialNetworks != null, () =>
            {
                RuleFor(p => p.Request.SocialNetworks!)
                    .MustBeVoCollection(
                    s => SocialNetwork.Create(s.URL, s.Platform),
                    ss => ListSocialNetwork.Create(ss));
            });

            When(p => p.Request.DonationsInfo != null, () =>
            {
                RuleFor(p => p.Request.DonationsInfo!)
                    .MustBeVoCollection(
                    d => DonationInfo.Create(d.Title, d.Description),
                    ds => ListDonationInfo.Create(ds));
            });
        }
    }
}