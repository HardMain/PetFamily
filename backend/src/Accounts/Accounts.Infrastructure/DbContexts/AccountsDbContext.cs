using Accounts.Domain.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure.DbContexts
{
    public class AccountsDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<AdminAccount> AdminAccounts => Set<AdminAccount>();
        public DbSet<ParticipantAccount> ParticipantAccounts => Set<ParticipantAccount>();
        public DbSet<VolunteerAccount> VolunteerAccounts => Set<VolunteerAccount>();
        public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();

        public AccountsDbContext(DbContextOptions<AccountsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AccountsDbContext).Assembly);

            builder.Entity<IdentityUserClaim<Guid>>()
                .ToTable("user_claims");

            builder.Entity<IdentityUserToken<Guid>>()
                .ToTable("user_tokens");

            builder.Entity<IdentityUserLogin<Guid>>()
                .ToTable("user_logins");

            builder.Entity<IdentityUserRole<Guid>>()
                .ToTable("user_roles");

            builder.Entity<IdentityRoleClaim<Guid>>()
                .ToTable("role_claims");

            builder.HasDefaultSchema("accounts");
        }
    }
}