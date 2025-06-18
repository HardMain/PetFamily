using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        static private Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        private Email(string email) 
        { 
            Value = email;
        }

        public string Value { get; }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure<Email>("Email can not be empty!");

            if (!regex.IsMatch(email))
                return Result.Failure<Email>("Invalid email format!");

            return Result.Success(new Email(email));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
