using System.Text.RegularExpressions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Aggregates.PetManagement.ValueObjects
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
            if (string.IsNullOrWhiteSpace(email) || !regex.IsMatch(email))
                return Errors.General.ValueIsInvalid("Email");

            return new Email(email);
        }
    }
}
