using PetFamily.Application.Abstractions;
using PetFamily.Contracts.Requests.Species.Breeds;

namespace PetFamily.Application.Species.Commands.AddBreed
{
    public record AddBreedCommand(
        Guid SpeciesId,
        AddBreedRequest Request) : ICommand;
}
