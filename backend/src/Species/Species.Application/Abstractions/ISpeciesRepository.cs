using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using SpeciesEntity = Species.Domain.Entities.Species;

namespace Species.Application.Abstractions
{
    public interface ISpeciesRepository
    {
        Task<Result<Guid>> Add(SpeciesEntity species, CancellationToken cancellationToken);
        Task<Result<Guid>> Delete(SpeciesEntity species, CancellationToken cancellationToken = default);
        Task<Result> ExistsBreedInSpecies(SpeciesId speciesId, BreedId breedId, CancellationToken cancellationToken = default);
        Task<Result<SpeciesEntity>> GetById(SpeciesId speciesId, CancellationToken cancellationToken = default);
        Task<Result<Guid>> Save(SpeciesEntity species, CancellationToken cancellationToken);
    }
}