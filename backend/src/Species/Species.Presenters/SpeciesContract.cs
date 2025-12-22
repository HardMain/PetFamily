using Microsoft.EntityFrameworkCore;
using Species.Application.Abstractions;
using Species.Contracts;

namespace Species.Presenters
{
    public class SpeciesContract : ISpeciesContract
    {
        private readonly ISpeciesReadDbContext _speciesReadDbContext;
        public SpeciesContract(ISpeciesReadDbContext speciesReadDbContext)
        {
            _speciesReadDbContext = speciesReadDbContext;
        }

        public async Task<bool> BreedExistsInSpeciesAsync(Guid breedId, Guid speciesId)
        {
            return await _speciesReadDbContext.Breeds
                .AnyAsync(b => b.Id == breedId && b.SpeciesId == speciesId);
        }
    }
}