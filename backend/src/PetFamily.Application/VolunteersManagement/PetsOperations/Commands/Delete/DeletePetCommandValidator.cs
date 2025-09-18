using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Delete
{
    public class DeletePetCommandValidator : AbstractValidator<DeletePetCommand>
    {
        public DeletePetCommandValidator()
        {
            RuleFor(p => p.VolunteerId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("volunteerId"));

            RuleFor(p => p.PetId)
                .NotEmpty()
                .WithError(Errors.General.ValueIsRequired("petId"));
        }
    }
}