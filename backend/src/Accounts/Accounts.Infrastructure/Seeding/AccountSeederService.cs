using System.Text.Json;
using Accounts.Domain.DataModels;
using Accounts.Infrastructure.IdentityManagers;
using Accounts.Infrastructure.Options;
using Framework.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volunteers.Domain.ValueObjects;

namespace Accounts.Infrastructure.Seeding
{
    public class AccountSeederService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            AdminAccountManager adminAccountManager,
            PermissionManager permissionManager,
            RolePermissionManager rolePermissionManager,
            IOptions<AdminOptions> adminOptions,
            ILogger<AccountSeederService> logger)
    {
        public async Task SeedAsync(CancellationToken cancellationToken)
        {
            var json = await File.ReadAllTextAsync(FilePaths.Accounts);

            var seedData = JsonSerializer.Deserialize<RolePermissionConfig>(json)
                ?? throw new ApplicationException("Could not deserialize role permission config.");

            await SeedPermissions(seedData, cancellationToken);
            await SeedRoles(seedData);
            await SeedRolePermissions(seedData, cancellationToken);
            await SeedAdminAccount(cancellationToken);
        }

        private async Task SeedRolePermissions(RolePermissionConfig seedData, CancellationToken cancellationToken)
        {
            foreach (var roleName in seedData.Roles.Keys)
            {
                var role = await roleManager.FindByNameAsync(roleName);

                if (role == null)
                    continue;

                var rolePermissions = seedData.Roles[roleName];

                await rolePermissionManager
                    .AddRangeIfExist(role.Id, rolePermissions, cancellationToken);
            }

            logger.LogInformation("RolePermissions added to database.");
        }

        private async Task SeedRoles(RolePermissionConfig seedData)
        {
            foreach (var roleName in seedData.Roles.Keys)
            {
                var role = await roleManager.FindByNameAsync(roleName);

                if (role is null)
                    await roleManager.CreateAsync(new Role { Name = roleName });
            }

            logger.LogInformation("Roles added to database.");
        }

        private async Task SeedPermissions(RolePermissionConfig seedData, CancellationToken cancellationToken)
        {
            var permissionsToAdd = seedData.Permissions.SelectMany(permissionGroup => permissionGroup.Value);

            await permissionManager.AddRangeIfExist(permissionsToAdd, cancellationToken);

            logger.LogInformation("Permissions added to database.");
        }

        private async Task SeedAdminAccount(CancellationToken cancellationToken)
        {
            var existingAdmin = await userManager.FindByEmailAsync(adminOptions.Value.Email);
            if (existingAdmin != null)
            {
                logger.LogInformation("Admin account already exists, seeding skipped.");
                return;
            }    

            var adminUser = User.CreateUser(adminOptions.Value.UserName, adminOptions.Value.Email);

            var createResult = await userManager.CreateAsync(adminUser, adminOptions.Value.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => $"[{e.Code}] {e.Description}"));
                throw new ApplicationException($"Failed to create admin user: {errors}");
            }

            await userManager.AddToRoleAsync(adminUser, AdminAccount.ADMIN);

            var fullName = FullName.Create(adminOptions.Value.UserName, adminOptions.Value.UserName).Value;
            var adminAccount = new AdminAccount(fullName, adminUser);
            await adminAccountManager
                .CreateAdminAccount(adminAccount, cancellationToken);

            logger.LogInformation("Admin account successfully created for {Email}.", adminUser.Email);
        }
    }
}