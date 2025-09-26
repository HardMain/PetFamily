using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Shared.ValueObjects
{
    public record ListDonationInfo
    {
        private readonly List<DonationInfo> _donations;

        private ListDonationInfo() { }
        private ListDonationInfo(IEnumerable<DonationInfo> donations)
        {
            _donations = donations.ToList();
        }

        public IReadOnlyList<DonationInfo> Donations => _donations;

        public static ListDonationInfo CreateEmpty() => new ListDonationInfo(new List<DonationInfo>());

        public static Result<ListDonationInfo> Create(IEnumerable<DonationInfo> donations)
        {
            var duplicates = donations
                .GroupBy(d => d)
                .Where(gr => gr.Count() > 1);

            if (duplicates.Any())
                return Errors.DonationInfo.Duplicate();

            return new ListDonationInfo(donations);
        }
    }
}