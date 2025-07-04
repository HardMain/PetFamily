using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.Volunteers.ValueObjects
{
    public record SocialNetwork
    {
        private SocialNetwork() { }

        private SocialNetwork(string url, string platform)
        {
            URL = url;
            Platform = platform;
        }

        public string URL { get; } = default!;
        public string Platform { get; } = default!;

        public static Result<SocialNetwork> Create(string url, string platform)
        {
            if (string.IsNullOrWhiteSpace(url))
                return "Url can not be empty!";

            if (string.IsNullOrWhiteSpace(platform))
                return "Platform can not be empty!";

            return new SocialNetwork(url, platform);
        }
    }
}
