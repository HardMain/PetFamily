using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Species;

namespace PetFamily.Application.Species.Commands.Create
{
    public record CreateSpeciesCommand(
        CreateSpeciesRequest Request) : ICommand;
}