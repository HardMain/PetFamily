namespace Accounts.Domain.DataModels
{
    public class VolunteerAccount
    {
        public const string VOLUNTEER = nameof(VOLUNTEER);

        //ef core
        private VolunteerAccount() { }

        public VolunteerAccount(User user)
        {
            Id = Guid.NewGuid();
            User = user;
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        //favoritePets
    }
}
