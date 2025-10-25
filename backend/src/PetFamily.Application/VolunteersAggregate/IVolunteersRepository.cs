using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.VolunteersAggregate.Entities;
using PetFamily.Domain.VolunteersAggregate.ValueObjects;

namespace PetFamily.Application.VolunteersAggregate
{
    public interface IVolunteersRepository
    {
        Task<Result<Guid>> Add(Volunteer volunteer, CancellationToken cancellationToken);
        Task<Result<Guid>> Save(Volunteer volunteer, CancellationToken cancellationToken);
        Task<Result<Guid>> Delete(Volunteer volunteer, CancellationToken cancellationToken);

        Task<Result<Volunteer>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken);
        Task<Result<Volunteer>> GetByPhoneNumber(PhoneNumber number, CancellationToken cancellationToken);
        Task<Result<Volunteer>> GetByEmail(Email email, CancellationToken cancellationToken);
        Task<int> DeleteSoftDeletedEarlierThan(DateTime dateTime, CancellationToken cancellationToken);
        Task<Result<Volunteer>> GetByIdIncludingSoftDeleted(VolunteerId volunteerId, CancellationToken cancellationToken);
    }
}