using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Aggregates.PetManagement.ValueObjects
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
                return Errors.General.ValueIsInvalid("Url");

            if (string.IsNullOrWhiteSpace(platform))
                return Errors.General.ValueIsInvalid("Platform");

            return new SocialNetwork(url, platform);
        }
    }
}
