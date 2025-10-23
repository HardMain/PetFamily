using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.SpeciesAggregate.Entities;

namespace PetFamily.Application.SpeciesAggregate
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