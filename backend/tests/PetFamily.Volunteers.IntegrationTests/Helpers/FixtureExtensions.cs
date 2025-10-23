using AutoFixture;
using PetFamily.Application.SpeciesAggregate.Commands.AddBreed;
using PetFamily.Application.SpeciesAggregate.Commands.Create;
using PetFamily.Application.SpeciesAggregate.Commands.Delete;
using PetFamily.Application.SpeciesAggregate.Commands.DeleteBreed;
using PetFamily.Application.VolunteersAggregate.Commands.AddPet;
using PetFamily.Application.VolunteersAggregate.Commands.AddPetFiles;
using PetFamily.Application.VolunteersAggregate.Commands.Create;
using PetFamily.Application.VolunteersAggregate.Commands.Delete;
using PetFamily.Application.VolunteersAggregate.Commands.DeletePet;
using PetFamily.Application.VolunteersAggregate.Commands.DeletePetFiles;
using PetFamily.Application.VolunteersAggregate.Commands.MovePet;
using PetFamily.Application.VolunteersAggregate.Commands.Restore;
using PetFamily.Application.VolunteersAggregate.Commands.RestorePet;
using PetFamily.Application.VolunteersAggregate.Commands.SetMainPhotoPet;
using PetFamily.Application.VolunteersAggregate.Commands.UpdateDonationsInfo;
using PetFamily.Application.VolunteersAggregate.Commands.UpdateMainInfo;
using PetFamily.Application.VolunteersAggregate.Commands.UpdateMainInfoPet;
using PetFamily.Application.VolunteersAggregate.Commands.UpdatePetSupportStatus;
using PetFamily.Application.VolunteersAggregate.Commands.UpdateSocialNetworks;
using PetFamily.Application.VolunteersManagement.PetsOperations.Commands.SetMainPhoto;
using PetFamily.Application.VolunteersManagement.PetsOperations.Commands.Update;
using PetFamily.Contracts.DTOs.Shared;
using PetFamily.Contracts.DTOs.Species;
using PetFamily.Contracts.DTOs.Volunteers;
using PetFamily.Contracts.DTOs.Volunteers.Pets;
using PetFamily.Contracts.Requests.Species;
using PetFamily.Contracts.Requests.Species.Breeds;
using PetFamily.Contracts.Requests.Volunteers;
using PetFamily.Contracts.Requests.Volunteers.Pets;

namespace PetFamily.Volunteers.IntegrationTests.Helpers
{
    public static class FixtureExtensions
    {
        public static CreateVolunteerCommand CreateCreateVolunteerCommand(
            this Fixture fixture)
        {
            var request = fixture.Build<CreateVolunteerRequest>()
                .With(r => r.FullName, new FullNameDto("firstName", "lastName", "middleName"))
                .With(r => r.Email, "email@email.com")
                .With(r => r.PhoneNumber, "+79999999999")
                .With(r => r.ExperienceYears, 1)
                .Create();

            return new CreateVolunteerCommand(request);
        }

        public static DeleteVolunteerCommand CreateDeleteVolunteerCommand(
            this Fixture fixture,
            Guid volunteerId)
        {
            return new DeleteVolunteerCommand(volunteerId);
        }

        public static UpdateDonationsInfoCommand CreateUpdateDonationsInfoCommand(
            this Fixture fixture,
            Guid volunteerId)
        {
            var request = fixture.Build<UpdateDonationsInfoRequest>()
                .Create();

            return new UpdateDonationsInfoCommand(volunteerId, request);
        }

        public static UpdateMainInfoCommand CreateUpdateMainInfoCommand(
            this Fixture fixture,
            Guid volunteerId)
        {
            var request = fixture.Build<UpdateMainInfoRequest>()
                .With(r => r.FullName, new FullNameDto("firstName", "lastName", "middleName"))
                .With(r => r.PhoneNumber, "+79999999999")
                .With(r => r.Email, "email@email.com")
                .With(r => r.ExperienceYears, 1)
                .Create();

            return new UpdateMainInfoCommand(volunteerId, request);
        }

        public static UpdateSocialNetworksCommand CreateUpdateSocialNetworksCommand(
            this Fixture fixture,
            Guid volunteerId)
        {
            var request = fixture.Build<UpdateSocialNetworksRequest>()
                .Create();

            return new UpdateSocialNetworksCommand(volunteerId, request);
        }

        public static AddPetCommand CreateAddPetCommand(
            this IFixture fixture,
            Guid volunteerId,
            Guid speciesId,
            Guid breedId)
        {
            var request = fixture.Build<AddPetRequest>()
                .With(r => r.OwnerPhone, "+79999999999")
                .With(r => r.BirthDate, DateTime.UtcNow.AddYears(-1))
                .With(r => r.SpeciesAndBreed, new SpeciesAndBreedDto(speciesId, breedId))
                .With(r => r.Address, new AddressDto("street", "houseNumber", "city", "country"))
                .With(r => r.SupportStatus, PetSupportStatusDto.FoundHome)
                .Create();

            return new AddPetCommand(volunteerId, request);
        }

        public static DeletePetCommand CreateDeletePetCommand(
            this IFixture fixture,
            Guid volunteerId,
            Guid petId)
        {
            return new DeletePetCommand(volunteerId, petId);
        }

        public static MovePetCommand CreateMovePetCommand(
            this IFixture fixture,
            Guid volunteerId,
            Guid petId,
            int newPosition)
        {
            var request = fixture.Build<MovePetRequest>()
                .With(r => r.newPosition, newPosition)
                .Create();

            return new MovePetCommand(volunteerId, petId, request);
        }

        public static RestoreVolunteerCommand CreateRestoreVolunteerCommand(
            this IFixture fixture,
            Guid volunteerId)
        {
            return new RestoreVolunteerCommand(volunteerId);
        }

        public static RestorePetCommand CreateRestorePetCommand(
            this IFixture fixture,
            Guid volunteerId,
            Guid petId)
        {
            return new RestorePetCommand(volunteerId, petId);
        }

        public static SetPetMainPhotoCommand CreateSetPetMainPhotoCommand(
            this IFixture fixture,
            Guid volunteerId,
            Guid petId,
            string pathMainPhoto)
        {
            var request = fixture.Build<SetPetMainPhotoRequest>()
                .With(r => r.Path, pathMainPhoto)
                .Create();

            return new SetPetMainPhotoCommand(volunteerId, petId, request);
        }

        public static UpdatePetCommand CreateUpdatePetCommand(
            this IFixture fixture,
            Guid volunteerId,
            Guid petId,
            Guid speciesId,
            Guid breedId)
        {
            var request = fixture.Build<UpdatePetRequest>()
                .With(r => r.OwnerPhone, "+79999999999")
                .With(r => r.SpeciesAndBreed, new SpeciesAndBreedDto(speciesId, breedId))
                .With(r => r.Address, new AddressDto("street", "houseNumber", "city", "country"))
                .With(r => r.BirthDate, DateTime.UtcNow.AddYears(-1))
                .Create();

            return new UpdatePetCommand(volunteerId, petId, request);
        }

        public static UpdatePetSupportStatusCommand CreateUpdatePetSupportStatusCommand(
            this IFixture fixture,
            Guid volunteerId,
            Guid petId)
        {
            var request = new UpdatePetSupportStatusRequest(PetSupportStatusDto.NeedHome);

            return new UpdatePetSupportStatusCommand(volunteerId, petId, request);
        }

        public static AddPetFilesCommand CreateAddPetFilesCommand(
            this IFixture fixture,
            Guid volunteerId,
            Guid petId)
        {
            var files = new List<FileFormDto>
            {
                new FileFormDto(new MemoryStream(new byte[]{1, 2, 3, 4, 5}), $"{Guid.NewGuid()}.jpg"),
                new FileFormDto(new MemoryStream(new byte[]{2, 3, 4, 5, 6}), $"{Guid.NewGuid()}.mp4"),
                new FileFormDto(new MemoryStream(new byte[]{3, 4, 5, 6, 7}), $"{Guid.NewGuid()}.jpeg"),
                new FileFormDto(new MemoryStream(new byte[]{4, 5, 6, 7, 8}), $"{Guid.NewGuid()}.png"),
            };

            return new AddPetFilesCommand(volunteerId, petId, files);
        }

        public static DeletePetFilesCommand CreateDeletePetFilesCommand(
            this IFixture fixture,
            Guid volunteerId,
            Guid petId,
            List<string> fileNames)
        {
            var request = new DeletePetFilesRequest(fileNames);

            return new DeletePetFilesCommand(volunteerId, petId, request);
        }

        public static CreateSpeciesCommand CreateCreateSpeciesCommand(
            this IFixture fixture)
        {
            var request = fixture.Build<CreateSpeciesRequest>()
                .Create();

            return new CreateSpeciesCommand(request);
        }

        public static DeleteSpeciesCommand CreateDeleteSpeciesCommand(
            this IFixture fixture,
            Guid speciesId)
        {
            return new DeleteSpeciesCommand(speciesId);
        }

        public static AddBreedCommand CreateAddBreedCommand(
            this IFixture fixture,
            Guid speciesId)
        {
            var request = fixture.Build<AddBreedRequest>()
                .Create();

            return new AddBreedCommand(speciesId, request);
        }

        public static DeleteBreedCommand CreateDeleteBreedCommand(
            this IFixture fixture,
            Guid speciesId,
            Guid breedId)
        {
            return new DeleteBreedCommand(speciesId, breedId);
        }
    }
}