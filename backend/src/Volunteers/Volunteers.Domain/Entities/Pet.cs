using Core.Abstractions;
using Core.Interfaces;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;
using SharedKernel.ValueObjects.Ids;
using Volunteers.Domain.Enums;
using Volunteers.Domain.ValueObjects;

namespace Volunteers.Domain.Entities
{
    public class Pet : Entity<PetId>, ISoftDeletable
    {
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
        public PetFile MainPhoto { get; private set; }
        public ListDonationInfo DonationsInfo { get; private set; } = ListDonationInfo.CreateEmpty();
        public IReadOnlyList<PetFile> Files => _files;

        public Position Position { get; private set; } = default!;
        public DateTime CreationDate { get; private set; }
        public DateTime? DeletionDate { get; private set; }
        public bool IsDeleted { get; private set; }

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

        public void AddFile(PetFile file) => _files.Add(file);
        public Result DeleteFile(PetFile file)
        {
            var isSuccess = _files.Remove(file);

            if (!isSuccess)
            {
                return Error.Failure("file.delete.db",
                    $"Fail to delete file with path {file.PathToStorage.Path}");
            }

            return Result.Success();
        }

        public void SetListDonationInfo(ListDonationInfo listDonationInfo)
            => DonationsInfo = listDonationInfo;
        public void SetPosition(Position position) =>
            Position = position;
        public Result<PetFile> SetMainPhoto(PetFile mainPhoto)
        {
            var existFile = _files.Any(f => f == mainPhoto);

            if (!existFile)
                return Errors.PetFile.NotFound(mainPhoto.PathToStorage.Path);

            MainPhoto = mainPhoto;

            return Result<PetFile>.Success(mainPhoto);
        }
        public void UpdateSupportStatus(SupportStatus supportStatus) =>
            SupportStatus = supportStatus;

        public void SoftDelete(bool cascade = false)
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

        public void Update(PetUpdateData petUpdateData)
        {
            Name = petUpdateData.Name;
            Description = petUpdateData.Description;
            SpeciesAndBreed = petUpdateData.SpeciesAndBreed;
            Color = petUpdateData.Color;
            HealthInformation = petUpdateData.HealthInformation;
            Address = petUpdateData.Address;
            WeightKg = petUpdateData.WeightKg;
            HeightCm = petUpdateData.HeightCm;
            OwnerPhone = petUpdateData.OwnerPhone;
            isCastrated = petUpdateData.isCastrated;
            isVaccinated = petUpdateData.isVaccinated;
            BirthDate = petUpdateData.BirthDate;
            DonationsInfo = petUpdateData.DonationsInfo;
        }
    }
}