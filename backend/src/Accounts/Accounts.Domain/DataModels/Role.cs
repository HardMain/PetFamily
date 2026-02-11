using Microsoft.AspNetCore.Identity;

namespace Accounts.Domain.DataModels
{
    public class Role : IdentityRole<Guid>
    {
        public List<RolePermission> RolePermissions { get; set; } = [];
    }
}