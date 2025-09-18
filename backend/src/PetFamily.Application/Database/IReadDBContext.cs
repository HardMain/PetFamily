using Microsoft.EntityFrameworkCore;
using PetFamily.Contracts.DTOs.Species;
using PetFamily.Contracts.DTOs.Volunteers;

namespace PetFamily.Infrastructure.DbContexts
{
    public interface IReadDbContext
    {
        IQueryable<VolunteerReadDto> Volunteers { get; }
        IQueryable<PetReadDto> Pets { get; }
        IQueryable<SpeciesReadDto> Species { get; }
        IQueryable<BreedReadDto> Breeds { get; }
    }
}