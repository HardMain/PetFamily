using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Commands.Delete
{
    public record DeleteSpeciesCommand(Guid Id) : ICommand;
}