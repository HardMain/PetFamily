using System.Security;
using PetFamily.Domain.Aggregates.PetManagement.Enums;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects.PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.Interfaces;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Domain.Aggregates.PetManagement.Entities
{
    public class Volunteer : Entity<VolunteerId>, ISoftDeletable
    {
        private readonly List<Pet> _pets = [];
        private readonly List<SocialNetwork> _socialNetworks = [];
        private readonly List<DonationInfo> _donationsInfo = [];

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
        public IReadOnlyList<SocialNetwork> SocialNetwork => _socialNetworks;
        public IReadOnlyList<DonationInfo> DonationsInfo => _donationsInfo;
        public IReadOnlyList<Pet> Pets => _pets;

        public bool IsDeleted { get; private set; }

        public DateTime? DeletionDate { get; private set; }

        public int CountPetsWithHome() => _pets.Count(pet => pet.SupportStatus == SupportStatus.found_home);
        public int CountPetsNeedHome() => _pets.Count(pet => pet.SupportStatus == SupportStatus.need_home);
        public int CountPetsNeedHelp() => _pets.Count(pet => pet.SupportStatus == SupportStatus.need_help);

        public Result<Pet> AddPet(Pet pet)
        {
            var serialNumberResult = SerialNumber.Create(_pets.Count() + 1);
            if (serialNumberResult.IsFailure)
                return serialNumberResult.Error;

            pet.SetSerialNumber(serialNumberResult.Value);

            _pets.Add(pet);

            return Result<Pet>.Success(pet);
        }
        public Result<Pet> DeletePet(Pet pet)
        {
            var serialNumber = pet.SerialNumber.Value;

            foreach (var petItem in _pets)
            {
                if (petItem.SerialNumber.Value > serialNumber)
                {
                    var newSerialResult = SerialNumber.Create(petItem.SerialNumber.Value - 1);
                    if (newSerialResult.IsFailure)
                        return newSerialResult.Error;

                    petItem.SetSerialNumber(newSerialResult.Value);
                }
            }

            _pets.Remove(pet);

            return Result<Pet>.Success(pet);
        }
        public Result<Pet> MovePet(Pet pet, SerialNumber newSerialNumber)
        {
            var newSerialNumberValue = newSerialNumber.Value;
            var currentSerialNumberValue = pet.SerialNumber.Value;

            if (newSerialNumberValue < 1 || newSerialNumberValue > _pets.Count)
                return Errors.General.ValueIsInvalid("serial number");

            if (newSerialNumberValue == currentSerialNumberValue)
                return Result<Pet>.Success(pet);

            if (newSerialNumber.Value > currentSerialNumberValue)
            {
                foreach (var petItem in _pets)
                {
                    if (petItem.SerialNumber.Value > currentSerialNumberValue &&
                    petItem.SerialNumber.Value <= newSerialNumberValue)
                    {
                        var serialNumberResult = SerialNumber.Create(petItem.SerialNumber.Value - 1);

                        if (serialNumberResult.IsFailure)
                            return serialNumberResult.Error;

                        petItem.SetSerialNumber(serialNumberResult.Value);
                    }
                }
            }
            else
            {
                foreach (var petItem in _pets)
                {
                    if (petItem.SerialNumber.Value < currentSerialNumberValue &&
                    petItem.SerialNumber.Value >= newSerialNumberValue)
                    {
                        var serialNumberResult = SerialNumber.Create(petItem.SerialNumber.Value + 1);

                        if (serialNumberResult.IsFailure)
                            return serialNumberResult.Error;

                        petItem.SetSerialNumber(serialNumberResult.Value);
                    }
                }
            }    

            pet.SetSerialNumber(newSerialNumber);
            
            return Result<Pet>.Success(pet);
        }

        private Result<SocialNetwork> AddSocialNetwork(SocialNetwork socialNetwork)
        {
            if (_socialNetworks.Any(sn => sn.URL == socialNetwork.URL))
                return Errors.SocialNetwork.Duplicate();

            _socialNetworks.Add(socialNetwork);

            return Result<SocialNetwork>.Success(socialNetwork);
        }
        public ErrorList AddSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
        {
            var errors = new List<Error>();

            foreach (var socialNetwork in socialNetworks)
            {
                var result = AddSocialNetwork(socialNetwork);
                if (result.IsFailure)
                    errors.Add(result.Error);
            }

            return new ErrorList(errors);
        }

        private Result<DonationInfo> AddDonationInfo(DonationInfo donationInfo)
        {
            if (_donationsInfo.Any(di => di == donationInfo))
                return Errors.DonationInfo.Duplicate();

            _donationsInfo.Add(donationInfo);

            return Result<DonationInfo>.Success(donationInfo);
        }
        public ErrorList AddDonationsInfo(IEnumerable<DonationInfo> donations)
        {
            var errors = new List<Error>();

            foreach (var donation in donations)
            {
                var result = AddDonationInfo(donation);
                if (result.IsFailure)
                    errors.Add(result.Error);
            }

            return new ErrorList(errors);
        }

        public void UpdateMainInfo(FullName name, Email email, PhoneNumber number, string description, int experienceYears) 
        {
            Name = name;
            Email = email;
            Number = number;
            Description = description;
            ExperienceYears = experienceYears;
        }
        public ErrorList UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
        {
            _socialNetworks.Clear();

            return AddSocialNetworks(socialNetworks);
        }
        public ErrorList UpdateDonationsInfo(IEnumerable<DonationInfo> donaitionsInfo)
        {
            _donationsInfo.Clear();

            return AddDonationsInfo(donaitionsInfo);
        }

        public Result<Pet> GetPetById(PetId petId) => _pets.First(p => p.Id == petId);

        public virtual void Delete(bool cascade = false)
        {
            if (IsDeleted)
                return;

            IsDeleted = true;

            DeletionDate = DateTime.UtcNow;

            if (cascade)
                _pets.ForEach(pet => pet.Delete());
        }
        public virtual void Restore(bool cascade = false)
        {
            if (!IsDeleted)
                return;

            IsDeleted = false;

            DeletionDate = null;

            if (cascade)
                _pets.ForEach(pet => pet.Restore());
        }
    }
}