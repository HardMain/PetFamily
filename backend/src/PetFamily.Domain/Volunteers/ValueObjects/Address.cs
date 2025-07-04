using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Volunteers.ValueObjects
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
                return "Street can not be empty!";

            if (string.IsNullOrWhiteSpace(houseNumber))
                return "HouseNumber can not be empty!";

            if (string.IsNullOrWhiteSpace(city))
                return "City can not be empty!";

            if (string.IsNullOrWhiteSpace(country))
                return "Country can not be empty!";

            return new Address(street, houseNumber, city, country);
        }
    }
}
