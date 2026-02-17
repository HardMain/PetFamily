using System.Data;
using Accounts.Domain.DataModels;
using Accounts.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure.IdentityManagers
{
    public class PermissionManager(AccountsDbContext accountsContext)
    {
        public async Task<Permission?> FindByCode(string code)
            => await accountsContext.Permissions.FirstOrDefaultAsync(p => p.Code == code);

        public async Task<HashSet<string>> GetUserPermissionCodesByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return await accountsContext.Users
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Roles)
                .SelectMany(r => r.RolePermissions)
                .Select(p => p.Permission.Code)
                .ToHashSetAsync(cancellationToken);
        }

        public async Task<HashSet<string>> GetUserPermissionCodesByRoles(IEnumerable<string> roles, CancellationToken cancellationToken)
        {
            return await accountsContext.Roles
                .Where(r => roles.Contains(r.Name))
                .SelectMany(r => r.RolePermissions)
                .Select(rp => rp.Permission.Code)
                .ToHashSetAsync(cancellationToken);
        }

        public async Task AddRangeIfExist(IEnumerable<string> permissions, CancellationToken cancellationToken)
        {
            foreach (var permissionCode in permissions)
            {
                var isPermissionExist = await accountsContext.Permissions
                    .AnyAsync(p => p.Code == permissionCode, cancellationToken);

                if (isPermissionExist)
                    continue;

                await accountsContext.Permissions
                    .AddAsync(new Permission() { Code = permissionCode }, cancellationToken);
            }

            await accountsContext.SaveChangesAsync(cancellationToken);
        }
    }
}