using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Species.Breeds;

namespace PetFamily.Application.SpeciesManagement.BreedsOperations.Commands.Add
{
    public record AddBreedCommand(
        Guid SpeciesId,
        AddBreedRequest Request) : ICommand;
}
