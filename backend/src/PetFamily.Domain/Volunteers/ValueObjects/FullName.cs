using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Volunteers.ValueObjects
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
                return "First name can not be empty!";

            if (string.IsNullOrWhiteSpace(lastName))
                return "Last name can not be empty!";

            return new FullName(firstName, lastName, middleName);
        }
    }
}
