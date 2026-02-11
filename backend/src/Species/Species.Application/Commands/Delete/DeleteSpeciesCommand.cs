using Core.Abstractions;

namespace Species.Application.Commands.Delete
{
    public record DeleteSpeciesCommand(Guid Id) : ICommand;
}