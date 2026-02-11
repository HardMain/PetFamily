namespace Accounts.Domain.DataModels
{
    public class RefreshSession
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public User User { get; set; } = default!;
        public Guid RefreshToken { get; init; }
        public Guid Jti { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime ExpiresIn { get; init; }
    }
}