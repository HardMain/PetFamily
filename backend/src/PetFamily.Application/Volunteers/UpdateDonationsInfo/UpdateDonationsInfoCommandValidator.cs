using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateDonationsInfo
{
    public class UpdateDonationsInfoCommandValidator : AbstractValidator<UpdateDonationsInfoCommand>
    {
        public UpdateDonationsInfoCommandValidator()
        {
            RuleForEach(di => di.Request.DonationsInfo)
                .MustBeValueObjects(di => DonationInfo.Create(di.Title, di.Description));
        }
    }
}
