using System.Text.Json;
using System.Text.Json.Serialization;
using Accounts.Presenters;
using Core.Caching;
using Microsoft.OpenApi.Models;
using Serilog;
using Species.Presenters;
using Volunteers.Presenters;

namespace Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProgramDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddWebDependencies(configuration)
                .AddVolunteersModule(configuration)
                .AddSpeciesModule(configuration)
                .AddAccountModule(configuration);
        }

        public static IServiceCollection AddWebDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSerilog();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                })
                .AddApplicationPart(typeof(AccountController).Assembly);

            services.AddEndpointsApiExplorer();

            services.AddSwagger();

            services.AddDistributedCache(configuration);

            return services;
        } 

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                    },
                    new string[] { }
                }
                });
            });
        }

        public static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                string connection = configuration.GetConnectionString("Redis")
                    ?? throw new InvalidOperationException("Connection string 'Redis' not found.");

                options.Configuration = connection;
            });

            services.AddSingleton<ICacheService, DistributedCacheService>();

            return services;
        }
    }
}