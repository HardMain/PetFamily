using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfoPet
{
    public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
    {
        public UpdatePetCommandValidator()
        {
            RuleFor(p => p.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("volunteerId"));

            RuleFor(p => p.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("petId"));

            RuleFor(v => v.Request.Name)
               .NotEmpty()
               .WithError(Errors.General.ValueIsRequired("name"))
               .MaximumLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
               .WithError(Errors.General.ValueIsInvalid("name"));

            RuleFor(p => p.Request.OwnerPhone)
                .MustBeValueObjects(PhoneNumber.Create);

            RuleFor(p => p.Request.Address)
                .MustBeValueObjects(p => Address.Create(p.Street, p.HouseNumber, p.City, p.Country));

            RuleFor(v => v.Request.HealthInformation)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("healthInformation"))
                .MaximumLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .WithError(Errors.General.ValueIsInvalid("healthInformation"));

            RuleFor(v => v.Request.Color)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("color"))
                .MaximumLength(Constants.MAX_LOW_TEXT_LENGTH)
                .WithError(Errors.General.ValueIsInvalid("color"));

            RuleFor(v => v.Request.Description)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("description"))
                .MaximumLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .WithError(Errors.General.ValueIsInvalid("description"));

            RuleFor(v => v.Request.BirthDate)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("birthDate"))
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithError(Errors.General.ValueIsInvalid("birthDate"));

            RuleFor(v => v.Request.WeightKg)
                .GreaterThanOrEqualTo(0)
                .WithError(Errors.General.ValueIsInvalid("weightKg"));

            RuleFor(v => v.Request.HeightCm)
                .GreaterThanOrEqualTo(0)
                .WithError(Errors.General.ValueIsInvalid("heightCm"));

            RuleFor(p => p.Request.DonationsInfo)
                .MustBeVoCollection(
                d => DonationInfo.Create(d.Title, d.Description),
                ds => ListDonationInfo.Create(ds));
        }
    }
}