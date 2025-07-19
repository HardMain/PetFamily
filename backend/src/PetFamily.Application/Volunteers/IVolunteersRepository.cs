using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.Volunteers.Entities;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers
{
    public interface IVolunteersRepository
    {
        Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken);

        Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken);
        Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber number, CancellationToken cancellationToken);
        Task<Result<Volunteer, Error>> GetByEmail(Email email, CancellationToken cancellationToken);
    }
}
