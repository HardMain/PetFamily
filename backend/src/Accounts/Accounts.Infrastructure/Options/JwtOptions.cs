namespace Accounts.Infrastructure.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; init; } = default!;
        public string Audience { get; init; } = default!;
        public string Key { get; init; } = default!;
        public string ExpiredMinutesTime { get; init; } = default!;
    }
}