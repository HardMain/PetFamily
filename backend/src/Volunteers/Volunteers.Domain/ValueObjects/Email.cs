using System.Text.RegularExpressions;
using SharedKernel.Failures;

namespace Volunteers.Domain.ValueObjects
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
