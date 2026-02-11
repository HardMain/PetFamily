using Accounts.Application;
using Accounts.Application.Abstractions;
using Accounts.Domain.DataModels;
using Accounts.Infrastructure;
using Accounts.Infrastructure.DbContexts;
using Accounts.Infrastructure.IdentityManagers;
using Accounts.Infrastructure.Jwt;
using Accounts.Infrastructure.Options;
using Accounts.Infrastructure.Seeding;
using Framework.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthorization(configuration)
                .AddDbContexts(configuration)
                .AddOptions(configuration)
                .AddProviders();

            return services;
        }

        public static IServiceCollection AddAuthorization(
            this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                ?? throw new InvalidOperationException("Missing jwt configuration");                

            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;  
            })
            .AddEntityFrameworkStores<AccountsDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = TokenValidationParametersFactory.CreateWithLifeTime(jwtOptions);
            });

            services.AddAuthorization();

            services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            services.AddSingleton<AccountsSeeder>();
            services.AddScoped<AccountSeederService>();

            services.AddScoped<PermissionManager>();
            services.AddScoped<RolePermissionManager>();
            services.AddScoped<AdminAccountManager>();
            services.AddScoped<RefreshSessionManager>();
            services.AddScoped<IRefreshSessionManager, RefreshSessionManager>();

            return services;
        }

        public static IServiceCollection AddDbContexts(
            this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database")
                ?? throw new InvalidOperationException("Connection string 'Database' not found.");

            services.AddDbContext<AccountsDbContext>(options => options.UseNpgsql(connectionString));

            return services;
        }

        public static IServiceCollection AddOptions(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.SectionName));
            services.Configure<RefreshOptions>(configuration.GetSection(RefreshOptions.SectionName));

            return services;
        }

        public static IServiceCollection AddProviders(
            this IServiceCollection services)
        {
            services.AddTransient<ITokenProvider, JwtTokenProvider>();

            return services;
        }
    }
}