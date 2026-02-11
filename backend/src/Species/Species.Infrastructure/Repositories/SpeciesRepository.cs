using Core.Caching;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Failures;
using SharedKernel.ValueObjects.Ids;
using Species.Application.Abstractions;
using Species.Infrastructure.DbContexts;
using SpeciesEntities = Species.Domain.Entities.Species;

namespace Species.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly SpeciesWriteDbContext _dbContext;
        private readonly ICacheService _cache;

        public SpeciesRepository(SpeciesWriteDbContext dbContext, ICacheService cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<Result<Guid>> Add(
            SpeciesEntities species, CancellationToken cancellationToken = default)
        {
            await _dbContext.Species.AddAsync(species, cancellationToken);

            var saveResult = await Save(species, cancellationToken);
            if (saveResult.IsFailure)
                return saveResult.Error;

            await _cache.RemoveByPrefixAsync($"species:name", cancellationToken);

            return species.Id.Value;
        }

        public async Task<Result<Guid>> Delete(
            SpeciesEntities species, CancellationToken cancellationToken = default)
        {
            _dbContext.Species.Remove(species);

            var saveResult = await Save(species, cancellationToken);
            if (saveResult.IsFailure)
                return saveResult.Error;

            await _cache.RemoveByPrefixAsync($"species:{species.Id.Value}", cancellationToken);
            await _cache.RemoveByPrefixAsync($"species:name", cancellationToken);

            return species.Id.Value;
        }

        public async Task<Result<SpeciesEntities>> GetById(
            SpeciesId speciesId, CancellationToken cancellationToken = default)
        {
            var species = await _dbContext.Species
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);

            if (species == null)
                return Errors.General.NotFound(speciesId);

            return species;
        }

        public async Task<Result<Guid>> Save(
            SpeciesEntities species, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);

                await _cache.RemoveByPrefixAsync($"species:{species.Id.Value}", cancellationToken);
                await _cache.RemoveByPrefixAsync("species:name", cancellationToken);

                return species.Id.Value;
            }
            catch (Exception)
            {
                return Errors.General.FailedToSave();
            }
        }

        public async Task<Result> ExistsBreedInSpecies(
            SpeciesId speciesId, BreedId breedId, CancellationToken cancellationToken = default)
        {
            var speciesAndBreed = await _dbContext.Species
                .AnyAsync(s => s.Id == speciesId && s.Breeds.Any(b => b.Id == breedId));

            if (!speciesAndBreed)
                return Errors.SpeciesAndBreed.NotFound(speciesId, breedId);

            return Result.Success();
        }
    }
}