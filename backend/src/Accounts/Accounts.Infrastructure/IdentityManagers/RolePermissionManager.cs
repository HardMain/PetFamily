using Accounts.Domain.DataModels;
using Accounts.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure.IdentityManagers
{
    public class RolePermissionManager(AccountsDbContext accountsContext)
    {
        public async Task AddRangeIfExist(Guid roleId, IEnumerable<string> permissions, CancellationToken cancellationToken)
        {
            foreach (var permissionCode in permissions)
            {
                var permission = await accountsContext.Permissions
                    .FirstOrDefaultAsync(p => p.Code == permissionCode, cancellationToken);

                if (permission is null)
                    throw new InvalidOperationException("Permission in DB not found. Check JSON!");

                var rolePermissionExist = await accountsContext.RolePermissions
                    .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id, cancellationToken);

                if (rolePermissionExist)
                    continue;

                accountsContext.RolePermissions.Add(new RolePermission() 
                { 
                    RoleId = roleId, 
                    PermissionId = permission!.Id 
                });
            }

            await accountsContext.SaveChangesAsync(cancellationToken);
        }
    }
}