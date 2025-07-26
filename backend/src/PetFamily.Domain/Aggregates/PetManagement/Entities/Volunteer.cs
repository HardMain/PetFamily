using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Aggregates.PetManagement.Enums;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Domain.Aggregates.PetManagement.Entities
{
    public class Volunteer : Entity<VolunteerId>
    {
        private readonly List<Pet> _pets = [];
        private readonly List<SocialNetwork> _socialNetworks = [];  //мб надо будет сделать обертку над списком, если запрос не сработает
        private readonly List<DonationInfo> _donationsInfo = [];    //тоже самое 

        private Volunteer(VolunteerId id) : base(id) { }

        private Volunteer(VolunteerId volunteerId, FullName name, Email email, string description, int experienceYears, PhoneNumber number) : base(volunteerId)
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

        public int CountPetsWithHome() => _pets.Count(pet => pet.SupportStatus == SupportStatus.found_home);
        public int CountPetsNeedHome() => _pets.Count(pet => pet.SupportStatus == SupportStatus.need_home);
        public int CountPetsNeedHelp() => _pets.Count(pet => pet.SupportStatus == SupportStatus.need_help);

        public static Result<Volunteer> Create(
            VolunteerId volunteerId,
            FullName name,
            Email email,
            string description,
            int experienceYears,
            PhoneNumber number)
        {
            if (string.IsNullOrWhiteSpace(description))
                return Errors.General.ValueIsInvalid("description");

            return new Volunteer(volunteerId, name, email, description, experienceYears, number);
        }

        //public UnitResult<Error> AddDonationsInfo(DonationInfo donation)
        //{
        //    if (donation == null)
        //        return Errors.General.ValueIsInvalid("donations");

        //    _donationsInfo.Add(donation);

        //    return UnitResult<Error>.Success();
        //}
    }
}
