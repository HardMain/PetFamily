using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Volunteers.ValueObjects;
using PetFamily.Domain.Volunteers.Enums;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Volunteers.Entities
{
    public class Volunteer : Shared.Entity<VolunteerId>
    {
        private readonly List<SocialNetwork> _socialNetworks = [];
        private readonly List<DonationInfo> _donationsInfo = [];
        private readonly List<Pet> _pets = [];

        private Volunteer(VolunteerId id) : base(id) { }

        private Volunteer(VolunteerId volunteerId, FullName name, Email email, string description) : base(volunteerId)
        {
            Name = name;
            Email = email;
            Description = description;
        }

        public FullName Name { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public int ExperienceYears { get; private set; }
        public PhoneNumber Number { get; private set; } = default!;
        public IReadOnlyList<SocialNetwork> SocialNetwork => _socialNetworks;
        public IReadOnlyList<DonationInfo> DonationsInfo => _donationsInfo;
        public IReadOnlyList<Pet> Pets => _pets;

        public int CountPetsWithHome() => _pets.Count(pet => pet.SupportStatus == SupportStatus.found_home);
        public int CountPetsNeedHome() => _pets.Count(pet => pet.SupportStatus == SupportStatus.need_home);
        public int CountPetsNeedHelp() => _pets.Count(pet => pet.SupportStatus == SupportStatus.need_help);

        public static Result<Volunteer> Create(VolunteerId volunteerId, FullName name, Email email, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return "Description can not be empty!";

            return new Volunteer(volunteerId, name, email, description);
        }
    }
}
