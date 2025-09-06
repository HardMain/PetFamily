using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Aggregates.PetManagement.Entities;
using PetFamily.Domain.Aggregates.Species.Entities;

namespace PetFamily.Infrastructure
{
    public class WriteDbContext: DbContext
    {
        private readonly string _connectionString;

        public WriteDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Volunteer> Volunteers => Set<Volunteer>();
        public DbSet<Species> Species => Set<Species>();
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(_connectionString)
                .UseLoggerFactory(CreateLoggerFactory());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly);
        }

        private ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(builder => builder.AddConsole());
    }

    public class ReadDbContext : DbContext
    {
        private readonly string _connectionString;

        public WriteDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Volunteer> Volunteers => Set<Volunteer>();
        public DbSet<Species> Species => Set<Species>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(_connectionString)
                .UseLoggerFactory(CreateLoggerFactory());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly);
        }

        private ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(builder => builder.AddConsole());
    }
}
