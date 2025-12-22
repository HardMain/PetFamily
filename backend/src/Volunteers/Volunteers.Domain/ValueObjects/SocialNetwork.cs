using System.Text.Json.Serialization;
using SharedKernel.Failures;

namespace Volunteers.Domain.ValueObjects
{
    public record SocialNetwork
    {
        private SocialNetwork() { }

        [JsonConstructor]
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
