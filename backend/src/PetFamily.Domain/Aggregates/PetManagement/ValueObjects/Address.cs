using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Aggregates.PetManagement.ValueObjects
{
    public record Address
    {
        public Address(string street, string houseNumber, string city, string country)
        {
            Street = street;
            HouseNumber = houseNumber;
            City = city;
            Country = country;
        }

        public string Street { get; }
        public string HouseNumber { get; }
        public string City { get; }
        public string Country { get; }

        public static Result<Address> Create(string street, string houseNumber, string city, string country)
        {
            if (string.IsNullOrWhiteSpace(street))
                return Errors.General.ValueIsInvalid("Street");

            if (string.IsNullOrWhiteSpace(houseNumber))
                return Errors.General.ValueIsInvalid("HouseNumber");

            if (string.IsNullOrWhiteSpace(city))
                return Errors.General.ValueIsInvalid("City");

            if (string.IsNullOrWhiteSpace(country))
                return Errors.General.ValueIsInvalid("Country");

            return new Address(street, houseNumber, city, country);
        }
    }
}
