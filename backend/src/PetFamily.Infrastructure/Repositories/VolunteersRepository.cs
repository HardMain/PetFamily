using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Aggregates.PetManagement.Entities;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Infrastructure.Repositories
{
    public class VolunteersRepository : IVolunteersRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VolunteersRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken)
        {
            await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return volunteer.Id;
        }

        public async Task<Result<Volunteer>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken)
        {
            var volunteer = await _dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteerId);

            if (volunteer == null)
                return Errors.General.NotFound(volunteerId.Value);

            return volunteer;
        }

        public async Task<Result<Volunteer>> GetByPhoneNumber(PhoneNumber phoneNumber, CancellationToken cancellationToken)
        {
            var volunteer = await _dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Number.Value == phoneNumber.Value);

            if (volunteer == null)
                return Errors.General.NotFound();

            return volunteer;
        }

        public async Task<Result<Volunteer>> GetByEmail(Email email, CancellationToken cancellationToken)
        {
            var volunteer = await _dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Email.Value == email.Value);

            if (volunteer == null)
                return Errors.General.NotFound();

            return volunteer;
        }
    }
}
