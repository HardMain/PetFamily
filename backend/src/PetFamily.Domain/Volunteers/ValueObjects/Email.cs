using System.Text.RegularExpressions;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Volunteers.ValueObjects
{
    public record Email
    {
        static private Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        private Email() { }

        private Email(string email)
        {
            Value = email;
        }

        public string Value { get; } = default!;

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "Email can not be empty!";

            if (!regex.IsMatch(email))
                return "Invalid email format!";

            return new Email(email);
        }
    }
}
