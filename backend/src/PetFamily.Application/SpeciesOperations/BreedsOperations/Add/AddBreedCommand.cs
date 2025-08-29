using PetFamily.Contracts.Requests.Species.Breeds;

namespace PetFamily.Application.SpeciesOperations.BreedsOperations.Add
{
    public record AddBreedCommand(Guid SpeciesId, AddBreedRequest Request);
}
