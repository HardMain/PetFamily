using Core.Abstractions;
using Core.Interfaces;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;
using SharedKernel.ValueObjects.Ids;
using Volunteers.Domain.Enums;
using Volunteers.Domain.ValueObjects;

namespace Volunteers.Domain.Entities
{
    public class Volunteer : Entity<VolunteerId>, ISoftDeletable
    {
        private readonly List<Pet> _pets = [];

        private Volunteer(VolunteerId id) : base(id) { }

        public Volunteer(VolunteerId volunteerId,
            FullName name,
            Email email,
            string description,
            int experienceYears,
            PhoneNumber number) : base(volunteerId)
        {
            Name = name;
            Email = email;
            Description = description;
            ExperienceYears = experienceYears;
            Number = number;
        }

        public FullName Name { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public int ExperienceYears { get; private set; }
        public PhoneNumber Number { get; private set; } = default!;
        public ListSocialNetwork SocialNetworks { get; private set; } = ListSocialNetwork.CreateEmpty();
        public ListDonationInfo DonationsInfo { get; private set; } = ListDonationInfo.CreateEmpty();
        public IReadOnlyList<Pet> Pets => _pets;

        public DateTime? DeletionDate { get; private set; }
        public bool IsDeleted { get; private set; }

        public int CountPetsWithHome() => _pets.Count(pet => pet.SupportStatus == SupportStatus.FoundHome);
        public int CountPetsNeedHome() => _pets.Count(pet => pet.SupportStatus == SupportStatus.NeedHome);
        public int CountPetsNeedHelp() => _pets.Count(pet => pet.SupportStatus == SupportStatus.NeedHelp);

        public Result<Pet> AddPet(Pet pet)
        {
            var positionResult = Position.Create(_pets.Count() + 1);
            if (positionResult.IsFailure)
                return positionResult.Error;

            pet.SetPosition(positionResult.Value);

            _pets.Add(pet);

            return Result<Pet>.Success(pet);
        }
        public void UpdatePet(Pet pet, PetUpdateData petUpdateData)
            => pet.Update(petUpdateData);

        public Result<Pet> HardDeletePet(Pet pet)
        {
            var currentPosition = pet.Position.Value;

            foreach (var petItem in _pets)
            {
                if (petItem.Position.Value > currentPosition)
                {
                    var newPositionResult = Position.Create(petItem.Position.Value - 1);
                    if (newPositionResult.IsFailure)
                        return newPositionResult.Error;

                    petItem.SetPosition(newPositionResult.Value);
                }
            }

            _pets.Remove(pet);

            return Result<Pet>.Success(pet);
        }
        public Result<Pet> SoftDeletePet(Pet pet, bool cascade = false)
        {
            if (pet.IsDeleted)
                return Result<Pet>.Success(pet);

            var currentPosition = pet.Position.Value;

            foreach (var petItem in _pets)
            {
                if (petItem.Position.Value > currentPosition)
                {
                    var newPositionResult = Position.Create(petItem.Position.Value - 1);
                    if (newPositionResult.IsFailure)
                        return newPositionResult.Error;

                    petItem.SetPosition(newPositionResult.Value);
                }
            }

            pet.SetPosition(Position.None);
            pet.SoftDelete(cascade);

            return Result<Pet>.Success(pet);
        }

        public void UpdatePetSupportStatus(Pet pet, SupportStatus supportStatus)
            => pet.UpdateSupportStatus(supportStatus);

        public Result<PetFile> SetPetMainPhoto(Pet pet, PetFile mainPhoto)
            => pet.SetMainPhoto(mainPhoto);

        public Result RestorePet(Pet pet, bool cascade = false)
        {
            if (!pet.IsDeleted)
                return Result<Pet>.Success(pet);

            var positionResult = Position.Create(_pets.Count(p => !p.IsDeleted) + 1);
            if (positionResult.IsFailure)
                return positionResult.Error;

            pet.SetPosition(positionResult.Value);
            pet.Restore(cascade);

            return Result.Success();
        }
        public Result<Pet> MovePet(Pet pet, Position newPosition)
        {
            var newPositionValue = newPosition.Value;
            var currentPositionValue = pet.Position.Value;

            if (newPositionValue < 1)
                return Errors.General.ValueIsInvalid("position");

            if (newPositionValue == currentPositionValue)
                return Result<Pet>.Success(pet);

            if (newPositionValue > _pets.Count)
                newPositionValue = _pets.Count;

            if (newPositionValue > currentPositionValue)
            {
                foreach (var petItem in _pets)
                {
                    if (petItem.Position.Value > currentPositionValue &&
                        petItem.Position.Value <= newPositionValue)
                    {
                        var PositionResult = Position.Create(petItem.Position.Value - 1);

                        if (PositionResult.IsFailure)
                            return PositionResult.Error;

                        petItem.SetPosition(PositionResult.Value);
                    }
                }
            }
            else
            {
                foreach (var petItem in _pets)
                {
                    if (petItem.Position.Value < currentPositionValue &&
                        petItem.Position.Value >= newPositionValue)
                    {
                        var PositionResult = Position.Create(petItem.Position.Value + 1);

                        if (PositionResult.IsFailure)
                            return PositionResult.Error;

                        petItem.SetPosition(PositionResult.Value);
                    }
                }
            }

            pet.SetPosition(Position.Create(newPositionValue).Value);

            return Result<Pet>.Success(pet);
        }

        public Result AddFileToPet(PetId petId, PetFile file) => AddFilesToPet(petId, [file]);
        public Result AddFilesToPet(PetId petId, IEnumerable<PetFile> petFiles)
        {
            var pet = _pets.FirstOrDefault(pet => pet.Id == petId);
            if (pet == null)
                return Errors.General.NotFound(petId);

            foreach (var file in petFiles)
                pet.AddFile(file);

            return Result.Success();
        }
        private Result DeleteFileFromPet(PetId petId, PetFile file)
        {
            var pet = _pets.FirstOrDefault(pet => pet.Id == petId);
            if (pet == null)
                return Errors.General.NotFound(petId);

            pet.DeleteFile(file);

            return Result.Success();
        }
        public Result DeleteFilesFromPet(PetId petId, IEnumerable<PetFile> petFiles)
        {
            var errors = new List<Error>();

            foreach (var file in petFiles)
            {
                var result = DeleteFileFromPet(petId, file);
                if (result.IsFailure)
                    return result.Error;
            }

            return Result.Success();
        }
        public void SetListDonationInfoToPet(Pet pet, ListDonationInfo listDonationInfo)
            => pet.SetListDonationInfo(listDonationInfo);
        public Result<IReadOnlyList<PetFile>> GetPetFiles(PetId petId)
        {
            var pet = _pets.FirstOrDefault(p => p.Id == petId);
            if (pet == null)
                return Errors.General.NotFound(petId);

            return Result<IReadOnlyList<PetFile>>.Success(pet.Files);
        }
        public IReadOnlyList<PetFile> GetAllPetsFiles()
        {
            return _pets.SelectMany(pet => pet.Files).ToList().AsReadOnly();
        }
        public void SetListSocialNetwork(ListSocialNetwork listSocialNetwork)
            => SocialNetworks = listSocialNetwork;
        public void SetListDonationInfo(ListDonationInfo listDonationInfo)
            => DonationsInfo = listDonationInfo;

        public void UpdateMainInfo(
            FullName name,
            Email email,
            PhoneNumber number,
            string description,
            int experienceYears)
        {
            Name = name;
            Email = email;
            Number = number;
            Description = description;
            ExperienceYears = experienceYears;
        }

        public Result<Pet> GetPetById(PetId petId)
        {
            var pet = _pets.FirstOrDefault(p => p.Id == petId);
            if (pet == null)
                return Errors.General.NotFound(petId);

            return Result<Pet>.Success(pet);
        }

        public void SoftDelete(bool cascade = false)
        {
            if (IsDeleted)
                return;

            IsDeleted = true;

            DeletionDate = DateTime.UtcNow;

            if (cascade)
                _pets.ForEach(pet => SoftDeletePet(pet, cascade));
        }
        public void Restore(bool cascade = false)
        {
            if (!IsDeleted)
                return;

            IsDeleted = false;

            DeletionDate = null;

            if (cascade)
                _pets.ForEach(pet => RestorePet(pet, cascade));
        }
    }
}