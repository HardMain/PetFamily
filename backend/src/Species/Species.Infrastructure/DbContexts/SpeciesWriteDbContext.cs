using Microsoft.EntityFrameworkCore;
using SpeciesEntities = Species.Domain.Entities.Species;

namespace Species.Infrastructure.DbContexts
{
    public class SpeciesWriteDbContext : DbContext
    {
        public SpeciesWriteDbContext(DbContextOptions<SpeciesWriteDbContext> options) : base(options) { }

        public DbSet<SpeciesEntities> Species => Set<SpeciesEntities>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("species");

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SpeciesWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);
        }
    }
}