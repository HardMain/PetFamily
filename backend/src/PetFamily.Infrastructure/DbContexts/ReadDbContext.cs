using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Contracts.DTOs.Species;
using PetFamily.Contracts.DTOs.Volunteers;

namespace PetFamily.Infrastructure.DbContexts
{

    public class ReadDbContext : DbContext, IReadDbContext
    {
        private readonly IConfiguration _configuration;

        public ReadDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IQueryable<VolunteerReadDto> Volunteers => Set<VolunteerReadDto>();
        public IQueryable<PetReadDto> Pets => Set<PetReadDto>();
        public IQueryable<SpeciesReadDto> Species => Set<SpeciesReadDto>();
        public IQueryable<BreedReadDto> Breeds => Set<BreedReadDto>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(_configuration.GetConnectionString("Database"))
                .UseLoggerFactory(CreateLoggerFactory())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Read") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() => 
            LoggerFactory.Create(builder => builder.AddConsole());
    }
}
