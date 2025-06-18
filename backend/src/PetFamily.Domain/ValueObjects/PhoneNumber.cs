using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects
{
    internal class PhoneNumber : ValueObject
    {
        static private Regex regex = new Regex(@"^\+[1-9]\d{9,14}$");

        private PhoneNumber(string number) 
        {
            Number = number;
        }

        public string Number { get; }

        public Result<PhoneNumber> Create(string number)
        {
            if (string.IsNullOrWhiteSpace(Number))
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
