using Volunteers.Contracts.DTOs;

namespace Volunteers.Application.Abstractions
{
    public interface IVolunteersReadDbContext
    {
        IQueryable<VolunteerReadDto> Volunteers { get; }
        IQueryable<PetReadDto> Pets { get; }
    }
}