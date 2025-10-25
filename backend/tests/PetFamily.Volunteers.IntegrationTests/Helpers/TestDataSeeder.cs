using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using PetFamily.Domain.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.SpeciesAggregate.Entities;
using PetFamily.Domain.VolunteersAggregate.ValueObjects;
using PetFamily.Domain.VolunteersAggregate.Entities;
using PetFamily.Domain.VolunteersAggregate.Enums;
using PetFamily.Contracts.SpeciesAggregate.DTOs;

namespace PetFamily.Volunteers.IntegrationTests.Helpers
{
    public class TestDataSeeder
    {
        private readonly WriteDbContext _writeDbContext;
        public TestDataSeeder(WriteDbContext writeDbContext)
        {
            _writeDbContext = writeDbContext;
        }

        public async Task<Guid> InitVolunteer()
        {
            var uniqueEmail = $"email_{Guid.NewGuid():N}@email.com";
            var uniquePhone = $"+7999999{new Random().Next(1000, 9999)}";

            var volunteer = new Volunteer(
                VolunteerId.NewVolunteerId(),
                FullName.Create("firstName", "lastName", "middleName").Value,
                Email.Create(uniqueEmail).Value,
                "description",
                1,
                PhoneNumber.Create(uniquePhone).Value);

            await _writeDbContext.Volunteers.AddAsync(volunteer);
            await _writeDbContext.SaveChangesAsync();

            return volunteer.Id;
        }

        public async Task<Guid> InitVolunteer(Volunteer volunteer)
        {
            await _writeDbContext.Volunteers.AddAsync(volunteer);
            await _writeDbContext.SaveChangesAsync();

            return volunteer.Id;
        }

        public async Task<Guid> InitPet(Guid volunteerId, Guid speciesId, Guid breedId)
        {
            var speciesAndBreedDto = await InitSpeciesAndBreed();

            var volunteer = await _writeDbContext.Volunteers
                .FirstAsync(v => v.Id == volunteerId);

            var uniquePhone = $"+7999999{new Random().Next(1000, 9999)}";

            var pet = Pet.Create(
                PetId.NewPetId(),
                "name",
                "description",
                SpeciesAndBreed.Create(SpeciesId.Create(speciesId), BreedId.Create(breedId)).Value,
                "color",
                "healthInformation",
                Address.Create("street", "houseNumber", "city", "country").Value,
                1,
                1,
                PhoneNumber.Create(uniquePhone).Value,
                false,
                false,
                DateTime.UtcNow,
                SupportStatus.FoundHome
                ).Value;

            volunteer.AddPet(pet);

            await _writeDbContext.SaveChangesAsync();
            return pet.Id;
        }

        public async Task<SpeciesAndBreedDto> InitSpeciesAndBreed()
        {
            var species = Domain.SpeciesAggregate.Entities.Species.Create(SpeciesId.NewSpeciesId(), "species").Value;
            var breed = Breed.Create(BreedId.NewBreedId(), "breed").Value;

            species.AddBreed(breed);

            await _writeDbContext.Species.AddAsync(species);
            await _writeDbContext.SaveChangesAsync();

            return new SpeciesAndBreedDto(species.Id, breed.Id);
        }
        public async Task<SpeciesId> InitSpecies()
        {
            var species = Domain.SpeciesAggregate.Entities.Species.Create(SpeciesId.NewSpeciesId(), "species").Value;

            await _writeDbContext.Species.AddAsync(species);
            await _writeDbContext.SaveChangesAsync();

            return species.Id;
        }
    }
}
