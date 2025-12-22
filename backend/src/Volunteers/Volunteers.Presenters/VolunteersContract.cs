using Microsoft.EntityFrameworkCore;
using Volunteers.Application.Abstractions;
using Volunteers.Contracts;

namespace Volunteers.Presenters
{
    public class VolunteersContract : IVolunteersContract
    {
        private readonly IVolunteersReadDbContext _volunteersReadDbContext;

        public VolunteersContract(IVolunteersReadDbContext volunteersReadDbContext)
        {
            _volunteersReadDbContext = volunteersReadDbContext;
        }

        public async Task<bool> IsBreedInUseAsync(Guid BreedId)
        {
            return await _volunteersReadDbContext.Pets.AnyAsync(p => p.SpeciesAndBreed.BreedId == BreedId);
        }

        public async Task<bool> IsSpeciesInUseAsync(Guid SpeciesId)
        {
            return await _volunteersReadDbContext.Pets.AnyAsync(p => p.SpeciesAndBreed.SpeciesId == SpeciesId);
        }
    }
}