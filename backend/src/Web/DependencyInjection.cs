using System.Text.Json;
using System.Text.Json.Serialization;
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
                .AddSpeciesModule(configuration);
        }

        public static IServiceCollection AddWebDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSerilog();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            return services;
        }
    }
}