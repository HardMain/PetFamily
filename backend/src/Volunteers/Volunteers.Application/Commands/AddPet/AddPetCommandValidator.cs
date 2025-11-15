using FluentValidation;
using Framework.Validation;
using SharedKernel.Constants;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;
using Volunteers.Domain.ValueObjects;

namespace Volunteers.Application.Commands.AddPet
{
    public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
    {
        public AddPetCommandValidator()
        {
            RuleFor(p => p.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("volunteerId"));

            RuleFor(p => p.Request.Name)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("name"))
                .MaximumLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                .WithError(Errors.General.ValueIsInvalid("name"));

            RuleFor(p => p.Request.Address)
                .MustBeValueObjects(adr => Address.Create(adr.Street, adr.HouseNumber, adr.City, adr.Country));

            RuleFor(p => p.Request.OwnerPhone)
                .MustBeValueObjects(PhoneNumber.Create);

            RuleFor(p => p.Request.SpeciesAndBreed)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("speciesAndBreed"));

            RuleFor(p => p.Request.SpeciesAndBreed.SpeciesId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("speciesId"));

            RuleFor(p => p.Request.SpeciesAndBreed.BreedId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("breedId"));

            RuleFor(p => p.Request.Color)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("color"))
                .MaximumLength(Constants.MAX_LOW_TEXT_LENGTH)
                .WithError(Errors.General.ValueIsInvalid("color"));

            RuleFor(p => p.Request.Description)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("description"))
                .MaximumLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .WithError(Errors.General.ValueIsInvalid("description"));

            RuleFor(p => p.Request.HealthInformation)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("healthInformation"))
                .MaximumLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .WithError(Errors.General.ValueIsInvalid("healthInformation"));

            RuleFor(p => p.Request.SupportStatus)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("supportStatus"));

            RuleFor(p => p.Request.BirthDate)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("birthDate"))
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithError(Errors.General.ValueIsInvalid("birthDate"));

            RuleFor(p => p.Request.WeightKg)
                .GreaterThanOrEqualTo(0)
                .WithError(Errors.General.ValueIsInvalid("weightKg"));

            RuleFor(p => p.Request.HeightCm)
                .GreaterThanOrEqualTo(0)
                .WithError(Errors.General.ValueIsInvalid("heightCm"));

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
