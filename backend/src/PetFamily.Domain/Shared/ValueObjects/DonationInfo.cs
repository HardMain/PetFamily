
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

        public static Result<DonationInfo> Create(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                return "Title can not be empty";

            if (string.IsNullOrWhiteSpace(description))
                return "Description can not be empty";

            return new DonationInfo(title, description);
        }
    }
}
