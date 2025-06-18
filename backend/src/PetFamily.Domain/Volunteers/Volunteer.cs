using CSharpFunctionalExtensions;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.Pets;

namespace PetFamily.Domain.Volunteers
{
    public class Volunteer : Entity
    {
        private readonly List<SocialNetwork> _socialNetworks = [];
        private readonly List<DonationInfo> _donationsInfo = [];
        private readonly List<Pet> _pets = [];

        private Volunteer(FullName name, Email email, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Description = description;
        }

        public Guid Id { get; private set; }
        public FullName Name { get; private set; }
        public Email Email { get; private set; }
        public string Description { get; private set; } = default!;
        public int ExperienceYears { get; private set; }
        public PhoneNumber Number { get; private set; } = default!;
        public IReadOnlyList<SocialNetwork> SocialNetwork => _socialNetworks;
        public IReadOnlyList<DonationInfo> DonationsInfo => _donationsInfo;
        public IReadOnlyList<Pet> Pets => _pets;

        public int CountPetsWithHome() => _pets.Count(pet => pet.SupportStatus == SupportStatus.found_home);
        public int CountPetsNeedHome() => _pets.Count(pet => pet.SupportStatus == SupportStatus.need_home);
        public int CountPetsNeedHelp() => _pets.Count(pet => pet.SupportStatus == SupportStatus.need_help);

        public static Result<Volunteer> Create(FullName name, Email email, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<Volunteer>("Description can not be empty!");

            return Result.Success(new Volunteer(name, email, description));
        }
    }
}
