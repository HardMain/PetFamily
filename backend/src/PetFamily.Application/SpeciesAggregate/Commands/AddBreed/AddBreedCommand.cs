using PetFamily.Application.Abstractions;
using PetFamily.Contracts.SpeciesAggregate.Requests;

namespace PetFamily.Application.SpeciesAggregate.Commands.AddBreed
{
    public record AddBreedCommand(
        Guid SpeciesId,
        AddBreedRequest Request) : ICommand;
}
