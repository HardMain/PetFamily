using Core.Caching;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;
using SharedKernel.ValueObjects.Ids;
using Volunteers.Application.Abstractions;
using Volunteers.Domain.Entities;
using Volunteers.Domain.ValueObjects;
using Volunteers.Infrastructure.DbContexts;

namespace Volunteers.Infrastructure.Repositories
{
    public class VolunteersRepository : IVolunteersRepository
    {
        private readonly VolunteersWriteDbContext _dbContext;
        private readonly ICacheService _cache;

        public VolunteersRepository(VolunteersWriteDbContext dbContext, ICacheService cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<Result<Guid>> Add(
            Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);

            var saveResult = await Save(volunteer, cancellationToken);
            if (saveResult.IsFailure)
                return saveResult.Error;

            return volunteer.Id.Value;
        }

        public async Task<Result<Guid>> Save(
            Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);

                await _cache.RemoveByPrefixAsync($"volunteer:{volunteer.Id.Value}", cancellationToken);

                foreach (var pet in volunteer.Pets)
                    await _cache.RemoveByPrefixAsync($"pet:{pet.Id.Value}", cancellationToken);

                return volunteer.Id.Value;
            }
            catch
            {
                return Errors.General.FailedToSave();
            }
        }

        public async Task<Result<Guid>> Delete(
            Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            _dbContext.Volunteers.Remove(volunteer);

            var saveResult = await Save(volunteer, cancellationToken);
            if (saveResult.IsFailure)
                return saveResult.Error;

            return volunteer.Id.Value;
        }

        public async Task<Result<Volunteer>> GetById(
            VolunteerId volunteerId, CancellationToken cancellationToken = default)
        {
            var volunteer = await _dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

            if (volunteer == null)
                return Errors.General.NotFound(volunteerId);

            return volunteer;
        }
        public async Task<Result<Volunteer>> GetByIdIncludingSoftDeleted(
            VolunteerId volunteerId, CancellationToken cancellationToken = default)
        {
            var volunteer = await _dbContext.Volunteers
                .IgnoreQueryFilters()
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

            if (volunteer == null)
                return Errors.General.NotFound(volunteerId.Value);

            return volunteer;
        }

        public async Task<Result<Volunteer>> GetByPhoneNumber(
            PhoneNumber phoneNumber, CancellationToken cancellationToken = default)
        {
            var volunteer = await _dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Number.Value == phoneNumber.Value, cancellationToken);

            if (volunteer == null)
                return Errors.General.NotFound();

            return volunteer;
        }

        public async Task<Result<Volunteer>> GetByEmail(
            Email email, CancellationToken cancellationToken = default)
        {
            var volunteer = await _dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Email.Value == email.Value);

            if (volunteer == null)
                return Errors.General.NotFound();

            return volunteer;
        }

        public async Task<int> DeleteSoftDeletedEarlierThan(
            DateTime dateTime, CancellationToken cancellationToken = default)
        {
            var count = await _dbContext.Volunteers.IgnoreQueryFilters()
                .Where(v => v.IsDeleted && v.DeletionDate <= dateTime)
                .ExecuteDeleteAsync(cancellationToken);

            return count;
        }
    }
}