using Microsoft.EntityFrameworkCore;
using Volunteers.Application.Abstractions;
using Volunteers.Contracts.DTOs;

namespace Volunteers.Infrastructure.DbContexts
{

    public class VolunteersReadDbContext : DbContext, IVolunteersReadDbContext
    {
        public VolunteersReadDbContext(DbContextOptions<VolunteersReadDbContext> options) : base(options) { }

        public IQueryable<VolunteerReadDto> Volunteers => Set<VolunteerReadDto>();
        public IQueryable<PetReadDto> Pets => Set<PetReadDto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("volunteers");

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(VolunteersReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }
    }
}