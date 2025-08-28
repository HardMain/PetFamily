using PetFamily.Domain.Aggregates.PetManagement.Enums;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects.PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Aggregates.SpeciesManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.Interfaces;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Aggregates.PetManagement.Entities
{
    public class Pet : Entity<PetId>, ISoftDeletable
    {
        private readonly List<DonationInfo> _donationsInfo = [];
        private readonly List<PetFile> _files = [];

        private Pet(PetId id) : base(id) { }

        private Pet(
            PetId id, 
            string name, 
            string description,
            SpeciesAndBreed speciesAndBreed,
            string color,
            string healthInformation,
            Address address,
            double weightKg,
            double heightCm,
            PhoneNumber ownerPhone,
            bool isCastrated,
            bool isVaccinated,
            DateTime birthDate,
            SupportStatus supportStatus) : base(id)
        {
            Name = name;
            Description = description;
            SpeciesAndBreed = speciesAndBreed;
            Color = color; 
            HealthInformation = healthInformation;
            Address = address;
            WeightKg = weightKg;
            HeightCm = heightCm;
            OwnerPhone = ownerPhone;
            this.isCastrated = isCastrated;
            this.isVaccinated = isVaccinated;
            BirthDate = birthDate;
            SupportStatus = supportStatus;

            CreationDate = DateTime.UtcNow;
        }

        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public SpeciesAndBreed SpeciesAndBreed { get; private set; } = default!;
        public string Color { get; private set; } = default!;
        public string HealthInformation { get; private set; } = default!;
        public Address Address { get; private set; } = default!;
        public double WeightKg { get; private set; }
        public double HeightCm { get; private set; }
        public PhoneNumber OwnerPhone { get; private set; } = default!;
        public bool isCastrated { get; private set; }
        public DateTime BirthDate { get; private set; } = default!;
        public bool isVaccinated { get; private set; }
        public SupportStatus SupportStatus { get; private set; }
        public IReadOnlyList<DonationInfo> DonationsInfo => _donationsInfo;
        public IReadOnlyList<PetFile> Files => _files;

        public SerialNumber SerialNumber { get; private set; } = default!;
        public bool IsDeleted { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime? DeletionDate { get; private set; }

        public static Result<Pet> Create(
            PetId id,
            string name,
            string description,
            SpeciesAndBreed speciesAndBreed,
            string color,
            string healthInformation,
            Address address,
            double weightKg,
            double heightCm,
            PhoneNumber ownerPhone,
            bool isCastrated,
            bool isVaccinated,
            DateTime birthDate,
            SupportStatus supportStatus)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Errors.General.ValueIsInvalid("Name");

            if (string.IsNullOrWhiteSpace(description))
                return Errors.General.ValueIsInvalid("Description");

            return new Pet(
                id, 
                name, 
                description, 
                speciesAndBreed, 
                color, 
                healthInformation, 
                address, 
                weightKg, 
                heightCm, 
                ownerPhone, 
                isCastrated, 
                isVaccinated, 
                birthDate, 
                supportStatus);
        }

        private void AddFile(PetFile file)
        {
            _files.Add(file);
        }
        public void AddFiles(IEnumerable<PetFile> files)
        {
            foreach (var file in files)
                AddFile(file);
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
        private Result DeleteFile(PetFile file)
        {
            var isSuccess = _files.Remove(file);

            if (!isSuccess)
            {
                return Error.Failure("file.delete.db", 
                    $"Fail to delete file with path {file.PathToStorage.Path}");
            }

            return Result.Success();
        }
        public Result<IReadOnlyList<PetFile>> DeleteFiles(IEnumerable<PetFile> files)
        {
            foreach(var file in files)
            {
                var result = DeleteFile(file);
                if (result.IsFailure)
                    return result.Error;
            }

            return files.ToList();
        }

        public void SetSerialNumber(SerialNumber serialNumber) =>
            SerialNumber = serialNumber;

        public void Delete(bool cascade = false)
        {
            if (IsDeleted)
                return;

            IsDeleted = true;

            DeletionDate = DateTime.UtcNow;
        }

        public void Restore(bool cascade = false)
        {
            if (!IsDeleted)
                return;

            IsDeleted = false;

            DeletionDate = null;
        }
    }
}