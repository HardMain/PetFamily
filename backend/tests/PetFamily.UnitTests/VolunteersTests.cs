using FluentAssertions;
using PetFamily.Domain.Aggregates.PetManagement.Entities;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects.PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.UnitTests
{
    public class VolunteersTests
    {
        private Volunteer CreateVolunteerWithPets(int petsCount)
        {
            var name = FullName.Create("first", "last", "middle").Value;
            var email = Email.Create("email@gmail.com").Value;
            var description = "description";
            var experienceYears = 10;
            var numberPhone = PhoneNumber.Create("+799999999999").Value;

            var volunteer = new Volunteer(VolunteerId.NewVolunteerId(), name, email, description, experienceYears, numberPhone);

            var pets = Enumerable
                .Range(1, petsCount)
                .Select(_ => Pet.Create(PetId.NewPetId(), "name", description, numberPhone).Value);

            foreach (var pet in pets)
                volunteer.AddPet(pet);

            return volunteer;
        }

        [Fact]
        public void AddPet_WithEmptyPets_ReturnsSuccessResult()
        {
            // arrange
            var volunteer = CreateVolunteerWithPets(0);

            var number = PhoneNumber.Create("+79999999999").Value;
            var petId = PetId.NewPetId();
            var pet = Pet.Create(petId, "name", "description", number).Value;

            // act
            var result = volunteer.AddPet(pet);

            // assert
            var addedPetResult = volunteer.GetPetById(petId);

            addedPetResult.IsSuccess.Should().BeTrue();
            result.IsSuccess.Should().BeTrue();
            addedPetResult.Value.Id.Should().Be(pet.Id);
            addedPetResult.Value.SerialNumber.Should().Be(SerialNumber.First);
        }

        [Fact]
        public void AddPet_WithExistingPets_ReturnsSuccessResult()
        {
            // arrange
            const int petsCount = 5;

            var volunteer = CreateVolunteerWithPets(petsCount);

            var petId = PetId.NewPetId();
            var number = PhoneNumber.Create("+79999999999").Value;
            var petToAdd = Pet.Create(petId, "name", "description", number).Value;

            // act
            var result = volunteer.AddPet(petToAdd);

            // assert
            var addedPetResult = volunteer.GetPetById(petId);

            var serialNumber = SerialNumber.Create(petsCount + 1);

            result.IsSuccess.Should().BeTrue();
            addedPetResult.IsSuccess.Should().BeTrue();
            addedPetResult.Value.Id.Should().Be(petToAdd.Id);
            addedPetResult.Value.SerialNumber.Should().Be(serialNumber.Value);
        }

        [Fact]
        public void DeletePet_FromFirtsPosition_ReturnsSuccessResult()
        {
            // arrange
            const int petsCount = 5;

            var volunteer = CreateVolunteerWithPets(petsCount);
            var pets = volunteer.Pets;

            var petToDelete = pets[0];

            // act
            var result = volunteer.DeletePet(petToDelete);

            // assert
            result.IsSuccess.Should().BeTrue();
            pets.Count().Should().Be(petsCount - 1);
            pets[0].SerialNumber.Value.Should().Be(1);
            pets[1].SerialNumber.Value.Should().Be(2);
            pets[2].SerialNumber.Value.Should().Be(3);
            pets[3].SerialNumber.Value.Should().Be(4);
        }

        [Fact]
        public void DeletePet_FromLastPosition_ReturnsSuccessResult()
        {
            // arrange
            const int petsCount = 5;

            var volunteer = CreateVolunteerWithPets(petsCount);
            var pets = volunteer.Pets;

            var petToDelete = pets[4];

            // act
            var result = volunteer.DeletePet(petToDelete);

            // assert
            result.IsSuccess.Should().BeTrue();
            pets.Count().Should().Be(petsCount - 1);
            pets[0].SerialNumber.Value.Should().Be(1);
            pets[1].SerialNumber.Value.Should().Be(2);
            pets[2].SerialNumber.Value.Should().Be(3);
            pets[3].SerialNumber.Value.Should().Be(4);
        }

        [Fact]
        public void DeletePet_FromMiddlePosition_ReturnsSuccessResult()
        {
            // arrange
            const int petsCount = 5;

            var volunteer = CreateVolunteerWithPets(petsCount);
            var pets = volunteer.Pets;

            var petToDelete = pets[2];

            // act
            var result = volunteer.DeletePet(petToDelete);

            // assert
            result.IsSuccess.Should().BeTrue();
            pets.Count().Should().Be(petsCount - 1);
            pets[0].SerialNumber.Value.Should().Be(1);
            pets[1].SerialNumber.Value.Should().Be(2);
            pets[2].SerialNumber.Value.Should().Be(3);
            pets[3].SerialNumber.Value.Should().Be(4);
        }

        [Fact]
        public void MovePet_ToFirstPosition_ReturnsSuccessResult()
        {
            // arrange
            const int petsCount = 5;

            var volunteer = CreateVolunteerWithPets(petsCount);
            var pets = volunteer.Pets;

            var petToMove = pets[4];

            // act
            var result = volunteer.MovePet(petToMove, SerialNumber.First);

            // assert
            result.IsSuccess.Should().BeTrue();
            pets[0].SerialNumber.Value.Should().Be(2);
            pets[1].SerialNumber.Value.Should().Be(3);
            pets[2].SerialNumber.Value.Should().Be(4);
            pets[3].SerialNumber.Value.Should().Be(5);
            pets[4].SerialNumber.Value.Should().Be(1);
        }

        [Fact]
        public void MovePet_ToLastPosition_ReturnsSuccessResult()
        {
            // arrange
            const int petsCount = 5;

            var volunteer = CreateVolunteerWithPets(petsCount);
            var pets = volunteer.Pets;

            var petToMove = pets[0];
            var serialNumberToMove = SerialNumber.Create(petsCount).Value;

            // act
            var result = volunteer.MovePet(petToMove, serialNumberToMove);

            // assert
            result.IsSuccess.Should().BeTrue();
            pets[0].SerialNumber.Value.Should().Be(5);
            pets[1].SerialNumber.Value.Should().Be(1);
            pets[2].SerialNumber.Value.Should().Be(2);
            pets[3].SerialNumber.Value.Should().Be(3);
            pets[4].SerialNumber.Value.Should().Be(4);
        }

        [Fact]
        public void MovePet_ToMiddlePosition_ReturnsSuccessResult()
        {
            // arrange
            const int petsCount = 5;

            var volunteer = CreateVolunteerWithPets(petsCount);
            var pets = volunteer.Pets;

            var petToMove = pets[1];
            var serialNumberToMove = SerialNumber.Create(3).Value;

            // act
            var result = volunteer.MovePet(petToMove, serialNumberToMove);

            // assert
            result.IsSuccess.Should().BeTrue();
            pets[0].SerialNumber.Value.Should().Be(1);
            pets[1].SerialNumber.Value.Should().Be(3);
            pets[2].SerialNumber.Value.Should().Be(2);
            pets[3].SerialNumber.Value.Should().Be(4);
            pets[4].SerialNumber.Value.Should().Be(5);
        }

        [Fact]
        public void MovePet_ToCurrentPosition_ReturnsSuccessResult()
        {
            // arrange
            const int petsCount = 5;

            var volunteer = CreateVolunteerWithPets(petsCount);
            var pets = volunteer.Pets;

            var petToMove = pets[2];
            var serialNumberToMove = SerialNumber.Create(3).Value;

            // act
            var result = volunteer.MovePet(petToMove, serialNumberToMove);

            // assert
            result.IsSuccess.Should().BeTrue();
            pets[0].SerialNumber.Value.Should().Be(1);
            pets[1].SerialNumber.Value.Should().Be(2);
            pets[2].SerialNumber.Value.Should().Be(3);
            pets[3].SerialNumber.Value.Should().Be(4);
            pets[4].SerialNumber.Value.Should().Be(5);
        }

        [Fact]
        public void MovePet_WithSerialNumberOutOfRange_ReturnsFailureResult()
        {
            // arrange
            const int petsCount = 5;

            var volunteer = CreateVolunteerWithPets(petsCount);
            var pets = volunteer.Pets;

            var petToMove = pets[0];
            var serialNumberToMove = SerialNumber.Create(6).Value;

            // act
            var result = volunteer.MovePet(petToMove, serialNumberToMove);

            // assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Errors.General.ValueIsInvalid("serial number"));
        }
    }
}