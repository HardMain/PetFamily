namespace Accounts.Infrastructure.Options
{
    public class RefreshOptions
    {
        public const string SectionName = "RefreshSession";
        public string ExpiredDaysTime { get; init; } = default!;
    }
}