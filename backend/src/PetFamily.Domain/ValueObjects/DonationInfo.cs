using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects
{
    internal class DonationInfo : ValueObject
    {
        private DonationInfo(string title, string description) 
        {
            Title = title;
            Description = description;
        }

        public string Title { get;} = default!;
        public string Description { get; } = default!;

        public Result<DonationInfo> Create(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<DonationInfo>("Title can not be empty");

            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<DonationInfo>("Description can not be empty");

            return Result.Success(new DonationInfo(title, description));
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Title;
            yield return Description;
        }
    }
}
