using Microsoft.EntityFrameworkCore;
using PetFamily.Application.SpeciesOperations;
using PetFamily.Domain.Aggregates.Species.Entities;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SpeciesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Guid>> Add(
            Species species, CancellationToken cancellationToken = default)
        {
            await _dbContext.Species.AddAsync(species, cancellationToken);

            var saveResult = await Save(species, cancellationToken);
            if (saveResult.IsFailure)
                return saveResult.Error;

            return species.Id.Value;
        }
        
        public async Task<Result<Species>> GetById(
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
            Species species, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
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