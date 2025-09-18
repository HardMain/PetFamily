using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.Commands.Delete
{
    public record DeleteSpeciesCommand(Guid Id) : ICommand;
}