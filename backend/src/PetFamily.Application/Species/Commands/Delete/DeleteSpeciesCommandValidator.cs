using FluentValidation;

namespace PetFamily.Application.Species.Commands.Delete
{
    public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
    {
        public DeleteSpeciesCommandValidator()
        {

        }
    }
}
