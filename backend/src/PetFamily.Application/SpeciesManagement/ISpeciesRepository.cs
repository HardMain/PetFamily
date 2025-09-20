using PetFamily.Domain.Aggregates.Species.Entities;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.SpeciesManagement
{
    public interface ISpeciesRepository
    {
        Task<Result<Guid>> Add(Species species, CancellationToken cancellationToken);
        Task<Result<Guid>> Delete(Species species, CancellationToken cancellationToken = default);
        Task<Result> ExistsBreedInSpecies(SpeciesId speciesId, BreedId breedId, CancellationToken cancellationToken = default);
        Task<Result<Species>> GetById(SpeciesId speciesId, CancellationToken cancellationToken = default);
        Task<Result<Guid>> Save(Species species, CancellationToken cancellationToken);
    }
}