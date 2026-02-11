namespace Accounts.Infrastructure.Options
{
    public class AdminOptions
    {
        public const string SectionName = "Admin";
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}