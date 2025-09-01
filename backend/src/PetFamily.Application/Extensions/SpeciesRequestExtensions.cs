using PetFamily.Application.SpeciesOperations.BreedsOperations.Add;
using PetFamily.Application.SpeciesOperations.Create;
using PetFamily.Contracts.Requests.Species;
using PetFamily.Contracts.Requests.Species.Breeds;

namespace PetFamily.Application.Extensions
{
    public static class SpeciesRequestExtensions
    {
        public static CreateSpeciesCommand ToCommand(this CreateSpeciesRequest request)
        {
            return new CreateSpeciesCommand(request);
        }
        public static AddBreedCommand ToCommand(this AddBreedRequest request, Guid speciesId)
        {
            return new AddBreedCommand(speciesId, request);
        }
    }
}