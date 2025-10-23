using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Species;

namespace PetFamily.Application.SpeciesAggregate.Commands.Create
{
    public record CreateSpeciesCommand(
        CreateSpeciesRequest Request) : ICommand;
}