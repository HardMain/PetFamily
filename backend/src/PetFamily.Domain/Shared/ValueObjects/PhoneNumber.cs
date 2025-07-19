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
            Number = number;
        }

        public string Number { get; } = default!;

        public static Result<PhoneNumber> Create(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return "Number can not be empty!";

            if (!regex.IsMatch(number))
                return "Invalid phone number format!";

            return new PhoneNumber(number);
        }
    }
}
