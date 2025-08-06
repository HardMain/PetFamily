using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks
{
    public class UpdateSocialNetworksCommandValidator : AbstractValidator<UpdateSocialNetworksCommand>
    {
        public UpdateSocialNetworksCommandValidator()
        {
            RuleForEach(sn => sn.Request.SocialNetworks)
                .MustBeValueObjects(sn => SocialNetwork.Create(sn.URL, sn.Platform));
        }
    }
}
