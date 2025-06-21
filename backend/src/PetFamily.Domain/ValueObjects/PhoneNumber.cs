using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        static private Regex regex = new Regex(@"^\+[1-9]\d{9,14}$");

        private PhoneNumber(string number) 
        {
            Number = number;
        }

        public string Number { get; }

        public static Result<PhoneNumber> Create(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return Result.Failure<PhoneNumber>("Number can not be empty!");

            if (!regex.IsMatch(number))
                return Result.Failure<PhoneNumber>("Invalid phone number format!");

            return Result.Success(new PhoneNumber(number));
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }
    }
}
