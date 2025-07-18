using System.Text.RegularExpressions;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Shared.ValueObjects
{
    public record PhoneNumber
    {
        static private Regex regex = new Regex(@"^\+[1-9]\d{9,14}$");

        private PhoneNumber() { }

        private PhoneNumber(string number)
        {
            Value = number;
        }

        public string Value { get; } = default!;

        public static Result<PhoneNumber, Error> Create(string number)
        {
            if (string.IsNullOrWhiteSpace(number) || !regex.IsMatch(number))
                return Errors.General.ValueIsInvalid("Number");

            return new PhoneNumber(number);
        }
    }
}
