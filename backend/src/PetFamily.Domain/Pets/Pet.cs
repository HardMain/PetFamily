using CSharpFunctionalExtensions;
using PetFamily.Domain.ValueObjects;

namespace PetFamily.Domain.Pets
{
    public class Pet : Entity
    {
        private readonly List<DonationInfo> _donationInfo = [];

        private Pet() {}

        private Pet(string name, string description, PhoneNumber ownerPhone) 
        { 
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            OwnerPhone = ownerPhone;
        }

        public Guid Id { get; private set; }
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
        public DateOnly BirthDate { get; private set; } = default!;
        public bool isVaccinated { get; private set; }
        public SupportStatus SupportStatus { get; private set; }
        public IReadOnlyList<DonationInfo> DonationsInfo => _donationInfo;
        public DateTime CreationDate { get; private set; }

        public static Result<Pet> Create(string name, string description, PhoneNumber ownerPhone)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Pet>("Name can not be empty!");

            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<Pet>("Descrition can not be empty!");

            return Result.Success(new Pet(name, description, ownerPhone));
        }
    }
}