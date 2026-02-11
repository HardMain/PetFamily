using Microsoft.EntityFrameworkCore;
using Volunteers.Domain.Entities;

namespace Volunteers.Infrastructure.DbContexts
{
    public class VolunteersWriteDbContext : DbContext 
    {
        public VolunteersWriteDbContext(DbContextOptions<VolunteersWriteDbContext> options) : base(options) { }

        public DbSet<Volunteer> Volunteers => Set<Volunteer>(); 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("volunteers");

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(VolunteersWriteDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);
        }
    }
}