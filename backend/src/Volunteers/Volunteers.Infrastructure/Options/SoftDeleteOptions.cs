namespace Volunteers.Infrastructure.Options
{
    public class SoftDeleteOptions
    {
        public int RetentionDate { get; init; } = 30;
    }
}