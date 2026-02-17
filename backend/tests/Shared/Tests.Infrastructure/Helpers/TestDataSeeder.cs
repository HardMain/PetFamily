using Microsoft.EntityFrameworkCore;
using SharedKernel.ValueObjects;
using SharedKernel.ValueObjects.Ids;
using Species.Contracts.DTOs;
using Species.Domain.Entities;
using Species.Infrastructure.DbContexts;
using Volunteers.Domain.Entities;
using Volunteers.Domain.Enums;
using Volunteers.Domain.ValueObjects;
using Volunteers.Infrastructure.DbContexts;

namespace Tests.Infrastructure.Helpers
{
    public class TestDataSeeder
    {
        private readonly VolunteersWriteDbContext _volunteerWriteDbContext;
        private readonly SpeciesWriteDbContext _speciesWriteDbContext;
        public TestDataSeeder(
            VolunteersWriteDbContext volunteerWriteDbContext,
            SpeciesWriteDbContext speciesWriteDbContext)
        {
            _volunteerWriteDbContext = volunteerWriteDbContext;
            _speciesWriteDbContext = speciesWriteDbContext;
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

            await _volunteerWriteDbContext.Volunteers.AddAsync(volunteer);
            await _volunteerWriteDbContext.SaveChangesAsync();

            return volunteer.Id;
        }

        public async Task<Guid> InitVolunteer(Volunteer volunteer)
        {
            await _volunteerWriteDbContext.Volunteers.AddAsync(volunteer);
            await _volunteerWriteDbContext.SaveChangesAsync();

            return volunteer.Id;
        }

        public async Task<Guid> InitPet(Guid volunteerId, Guid speciesId, Guid breedId)
        {
            var speciesAndBreedDto = await InitSpeciesAndBreed();

            var volunteer = await _volunteerWriteDbContext.Volunteers
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

            await _volunteerWriteDbContext.SaveChangesAsync();
            return pet.Id;
        }

        public async Task<SpeciesAndBreedDto> InitSpeciesAndBreed()
        {
            var species = Species.Domain.Entities.Species.Create(SpeciesId.NewSpeciesId(), "species").Value;
            var breed = Breed.Create(BreedId.NewBreedId(), "breed").Value;

            species.AddBreed(breed);

            await _speciesWriteDbContext.Species.AddAsync(species);
            await _speciesWriteDbContext.SaveChangesAsync();

            return new SpeciesAndBreedDto(species.Id, breed.Id);
        }
        public async Task<SpeciesId> InitSpecies()
        {
            var species = Species.Domain.Entities.Species.Create(SpeciesId.NewSpeciesId(), "species").Value;

            await _speciesWriteDbContext.Species.AddAsync(species);
            await _speciesWriteDbContext.SaveChangesAsync();

            return species.Id;
        }
    }
}