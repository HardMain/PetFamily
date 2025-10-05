using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.UpdateSupportStatus
{
    public class UpdatePetSupportStatusCommandValidator : AbstractValidator<UpdatePetSupportStatusCommand>
    {
        public UpdatePetSupportStatusCommandValidator()
        {
            RuleFor(v => v.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("volunteerId"));

            RuleFor(v => v.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("petId"));

            RuleFor(v => v.Request.SupportStatus)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("supportStatus"));
        }
    }
}