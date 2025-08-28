using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.VolunteersOperations.UpdateDonationsInfo
{
    public class UpdateDonationsInfoCommandValidator : AbstractValidator<UpdateDonationsInfoCommand>
    {
        public UpdateDonationsInfoCommandValidator()
        {
            RuleFor(di => di.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(di => di.Request.DonationsInfo)
                .MustBeValueObjects(di => DonationInfo.Create(di.Title, di.Description));
        }
    }
}
