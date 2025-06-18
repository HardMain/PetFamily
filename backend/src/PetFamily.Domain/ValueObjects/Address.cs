using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects
{
    internal class Address : ValueObject
    {
        private Address(string street, string houseNumber, string city, string country)
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

        public Result<Address> Create(string street, string houseNumber, string city, string country)
        {
            if (string.IsNullOrWhiteSpace(Street))
                return Result.Failure<Address>("Street can not be empty!");

            if (string.IsNullOrWhiteSpace(HouseNumber))
                return Result.Failure<Address>("HouseNumber can not be empty!");

            if (string.IsNullOrWhiteSpace(City))
                return Result.Failure<Address>("City can not be empty!");

            if (string.IsNullOrWhiteSpace(Country))
                return Result.Failure<Address>("Country can not be empty!");

            return Result.Success(new Address(street, houseNumber, city, country));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return HouseNumber;
            yield return City;
            yield return Country;
        }
    }
}
