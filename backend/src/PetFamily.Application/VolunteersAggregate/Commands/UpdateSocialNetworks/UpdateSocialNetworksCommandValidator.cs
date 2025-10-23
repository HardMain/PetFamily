using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.VolunteersAggregate.ValueObjects;

namespace PetFamily.Application.VolunteersAggregate.Commands.UpdateSocialNetworks
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
