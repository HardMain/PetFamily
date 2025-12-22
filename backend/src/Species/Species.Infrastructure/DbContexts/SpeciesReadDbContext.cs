using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Species.Application.Abstractions;
using Species.Contracts.DTOs;

namespace Species.Infrastructure.DbContexts
{

    public class SpeciesReadDbContext : DbContext, ISpeciesReadDbContext
    {
        public SpeciesReadDbContext(DbContextOptions<SpeciesReadDbContext> options) : base(options) { }

        public IQueryable<SpeciesReadDto> Species => Set<SpeciesReadDto>();
        public IQueryable<BreedReadDto> Breeds => Set<BreedReadDto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)  
        {
            modelBuilder.HasDefaultSchema("species");

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SpeciesReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }
    }
}
