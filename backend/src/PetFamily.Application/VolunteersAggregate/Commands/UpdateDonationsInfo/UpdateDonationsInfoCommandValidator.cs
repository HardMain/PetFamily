using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.VolunteersAggregate.Commands.UpdateDonationsInfo
{
    public class UpdateDonationsInfoCommandValidator : AbstractValidator<UpdateDonationsInfoCommand>
    {
        public UpdateDonationsInfoCommandValidator()
        {
            RuleFor(di => di.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleFor(p => p.Request.DonationsInfo!)
                    .MustBeVoCollection(
                    d => DonationInfo.Create(d.Title, d.Description),
                    ds => ListDonationInfo.Create(ds));
        }
    }
}