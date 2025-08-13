using PetFamily.Domain.Aggregates.PetManagement.Enums;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Aggregates.SpeciesManagement.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.Interfaces;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Aggregates.PetManagement.Entities
{
    public class Pet : Entity<PetId>, ISoftDeletable
    {
        private readonly List<DonationInfo> _donationInfo = [];

        private Pet(PetId id) : base(id) { }

        private Pet(PetId id, string name, string description, PhoneNumber ownerPhone) : base(id)
        {
            Name = name;
            Description = description;
            OwnerPhone = ownerPhone;
            CreationDate = DateTime.UtcNow;
        }

        public Volunteer Volunteer { get; private set; } = default!;
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
        public IReadOnlyList<DonationInfo> DonationsInfo => _donationInfo;
        public DateTime CreationDate { get; private set; }

        public bool IsDeleted { get; private set; }

        public DateTime? DeletionDate { get; private set; }

        public static Result<Pet> Create(PetId id, string name, string description, PhoneNumber ownerPhone)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Errors.General.ValueIsInvalid("Name");

            if (string.IsNullOrWhiteSpace(description))
                return Errors.General.ValueIsInvalid("Description");

            return new Pet(id, name, description, ownerPhone);
        }

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