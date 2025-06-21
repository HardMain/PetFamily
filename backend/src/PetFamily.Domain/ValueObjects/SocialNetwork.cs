using CSharpFunctionalExtensions;

namespace PetFamily.Domain.ValueObjects
{
    public class SocialNetwork : ValueObject
    {
        private SocialNetwork(string url, string platform)
        {
            URL = url;
            Platform = platform;
        }

        public string URL { get; }
        public string Platform { get; }

        public static Result<SocialNetwork> Create(string url, string platform)
        {
            if (string.IsNullOrWhiteSpace(url))
                return Result.Failure<SocialNetwork>("Url can not be empty!");

            if (string.IsNullOrWhiteSpace(platform))
                return Result.Failure<SocialNetwork>("Platform can not be empty!");

            return Result.Success(new SocialNetwork(url, platform));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return URL;
            yield return Platform;
        }
    }
}
