using Core.Validation;
using FluentValidation;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;

namespace Volunteers.Application.Commands.UpdateDonationsInfo
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