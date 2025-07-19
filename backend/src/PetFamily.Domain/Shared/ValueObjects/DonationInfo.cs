using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Shared.ValueObjects
{
    public record DonationInfo
    {
        private DonationInfo() { }

        private DonationInfo(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public string Title { get; } = default!;
        public string Description { get; } = default!;

        public static Result<DonationInfo, Error> Create(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Errors.General.ValueIsInvalid("Title");

            if (string.IsNullOrWhiteSpace(description))
                return Errors.General.ValueIsInvalid("Description");

            return new DonationInfo(title, description);
        }
    }
}