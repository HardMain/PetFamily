using System.Security.Claims;
using Accounts.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization
{
    public class PermissionRequirementHandler(IServiceScopeFactory scopeFactory) : IAuthorizationHandler
    {
        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            using var scope = scopeFactory.CreateAsyncScope();

            var accountContract = scope.ServiceProvider.GetRequiredService<IAccountsContract>();

            var userRoles = context.User.Claims
                .Where(claim => claim.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            if (!userRoles.Any())
            {
                context.Fail();
                return;
            }

            var permissions = await accountContract.GetUserPermissionCodesByRoles(userRoles);

            var permissionRequirements = context.Requirements.OfType<PermissionAttribute>();

            foreach (var requirement in permissionRequirements)
            {
                if (permissions.Contains(requirement.Code))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}