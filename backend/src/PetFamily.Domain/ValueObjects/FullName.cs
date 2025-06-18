using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects
{
    internal class FullName : ValueObject
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

        public Result<FullName> Create(string firstName, string lastName, string middleName)
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                return Result.Failure<FullName>("First name can not be empty!");

            if (string.IsNullOrWhiteSpace(FirstName))
                return Result.Failure<FullName>("First name can not be empty!");

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
