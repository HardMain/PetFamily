using Microsoft.AspNetCore.Identity;

namespace Accounts.Domain.DataModels
{
    public class User : IdentityUser<Guid>
    {
        private User() { }

        private List<Role> _roles = [];
        public IReadOnlyList<Role> Roles => _roles;

        public static User CreateUser(string userName, string email)
        {
            return new User
            {
                UserName = userName,
                Email = email,
            };
        }
        public List<SocialNetwork> SocialNetworks { get; set; } = [];
    }
}