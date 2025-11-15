using SharedKernel.Failures;

namespace Volunteers.Domain.ValueObjects
{
    public class ListSocialNetwork
    {
        private readonly List<SocialNetwork> _socials;
        private ListSocialNetwork() { }
        private ListSocialNetwork(IEnumerable<SocialNetwork> socials)
        {
            _socials = socials.ToList();
        }

        public IReadOnlyList<SocialNetwork> Socials => _socials;

        public static ListSocialNetwork CreateEmpty() => new ListSocialNetwork(new List<SocialNetwork>());

        public static Result<ListSocialNetwork> Create(IEnumerable<SocialNetwork> socials)
        {
            var duplicates = socials
                .GroupBy(d => d)
                .Where(gr => gr.Count() > 1);

            if (duplicates.Any())
                return Errors.SocialNetwork.Duplicate();

            return new ListSocialNetwork(socials);
        }
    }
}