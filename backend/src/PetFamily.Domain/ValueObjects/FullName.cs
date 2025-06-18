using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects
{
    public class FullName : ValueObject
    {
        private FullName(string firstName, string lastName, string middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string MiddleName { get; }

        public static Result<FullName> Create(string firstName, string lastName, string middleName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Result.Failure<FullName>("First name can not be empty!");

            if (string.IsNullOrWhiteSpace(lastName))
                return Result.Failure<FullName>("Last name can not be empty!");

            if (string.IsNullOrWhiteSpace(middleName))
                return Result.Failure<FullName>("Middle name can not be empty!");

            return Result.Success(new FullName(firstName, lastName, middleName));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return MiddleName;
        }
    }
}
