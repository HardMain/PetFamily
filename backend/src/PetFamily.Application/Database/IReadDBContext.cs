using Microsoft.EntityFrameworkCore;
using PetFamily.Contracts.SpeciesAggregate.DTOs;
using PetFamily.Contracts.VolunteersAggregate.DTOs;

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