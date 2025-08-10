﻿using PetFamily.Domain.Aggregates.PetManagement.Entities;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.Volunteers
{
    public interface IVolunteersRepository
    {
        Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken);
        Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken);
        Task<Guid> Delete(Volunteer volunteer, CancellationToken cancellationToken);

        Task<Result<Volunteer>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken);
        Task<Result<Volunteer>> GetByPhoneNumber(PhoneNumber number, CancellationToken cancellationToken);
        Task<Result<Volunteer>> GetByEmail(Email email, CancellationToken cancellationToken);
        Task<IEnumerable<Guid>> DeleteSoftDeletedEarlierThan(DateTime dateTime, CancellationToken cancellationToken);
        Task<Result<Volunteer>> GetByIdIncludingDeleted(VolunteerId volunteerId, CancellationToken cancellationToken);
    }
}