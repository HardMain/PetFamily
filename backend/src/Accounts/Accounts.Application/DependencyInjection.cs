using Core.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddCommands()
                .AddQueries()
                .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }

        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
                .AddClasses(classes =>
                    classes.AssignableToAny(typeof(ICommandHandler<>), typeof(ICommandHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            return services;
        }

        public static IServiceCollection AddQueries(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
                .AddClasses(classes =>
                    classes.AssignableTo(typeof(IQueryHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}
