using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Aggregates.PetManagement.ValueObjects
{
    public record FullName
    {
        private FullName() { }

        private FullName(string firstName, string lastName, string? middleName = null)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }

        public string FirstName { get; } = default!;
        public string LastName { get; } = default!;
        public string? MiddleName { get; } = default!;

        public static Result<FullName> Create(string firstName, string lastName, string? middleName = null)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Errors.General.ValueIsInvalid("FirstName");

            if (string.IsNullOrWhiteSpace(lastName))
                return Errors.General.ValueIsInvalid("LastName");

            return new FullName(firstName, lastName, middleName);
        }
    }
}
