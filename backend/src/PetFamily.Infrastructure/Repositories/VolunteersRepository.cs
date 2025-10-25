using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteersAggregate;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.VolunteersAggregate.Entities;
using PetFamily.Domain.VolunteersAggregate.ValueObjects;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories
{
    public class VolunteersRepository : IVolunteersRepository
    {
        private readonly WriteDbContext _dbContext;

        public VolunteersRepository(WriteDbContext dbContext)
        {
            _dbContext = dbContext; 
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
            return await _dbContext.Volunteers.IgnoreQueryFilters()
                .Where(v => v.IsDeleted && v.DeletionDate <= dateTime)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}