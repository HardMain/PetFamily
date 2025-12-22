using FluentValidation;
using Framework.Validation;
using SharedKernel.Failures;
using Volunteers.Domain.ValueObjects;

namespace Volunteers.Application.Commands.UpdateSocialNetworks
{
    public class UpdateSocialNetworksCommandValidator : AbstractValidator<UpdateSocialNetworksCommand>
    {
        public UpdateSocialNetworksCommandValidator()
        {
            RuleFor(sn => sn.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired());

            RuleForEach(sn => sn.Request.SocialNetworks)
                .MustBeValueObjects(sn => SocialNetwork.Create(sn.URL, sn.Platform));
        }
    }
}
