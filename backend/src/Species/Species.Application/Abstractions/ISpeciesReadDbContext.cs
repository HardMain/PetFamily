using Species.Contracts.DTOs;

namespace Species.Application.Abstractions
{
    public interface ISpeciesReadDbContext
    {
        IQueryable<SpeciesReadDto> Species { get; }
        IQueryable<BreedReadDto> Breeds { get; }
    }
}