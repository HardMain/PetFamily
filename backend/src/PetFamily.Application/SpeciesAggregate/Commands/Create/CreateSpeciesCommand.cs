using PetFamily.Application.Abstractions;
using PetFamily.Contracts.SpeciesAggregate.Requests;

namespace PetFamily.Application.SpeciesAggregate.Commands.Create
{
    public record CreateSpeciesCommand(
        CreateSpeciesRequest Request) : ICommand;
}