using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesAggregate.Commands.Delete
{
    public record DeleteSpeciesCommand(Guid Id) : ICommand;
}