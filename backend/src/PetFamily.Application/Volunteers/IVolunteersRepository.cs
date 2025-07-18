using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.Volunteers.Entities;

namespace PetFamily.Application.Volunteers
{
    public interface IVolunteersRepository
    {
        Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken);

        Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId);
        Task<Result<Volunteer, Error>> GetByPhoneNumber(string number);
        Task<Result<Volunteer, Error>> GetByEmail(string email);
    }
}
