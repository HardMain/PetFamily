using FluentValidation;

namespace Species.Application.Commands.Delete
{
    public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
    {
        public DeleteSpeciesCommandValidator()
        {

        }
    }
}
