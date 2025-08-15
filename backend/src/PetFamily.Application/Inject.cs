using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.HardDelete;
using PetFamily.Application.Volunteers.PetsOperations.AddPets;
using PetFamily.Application.Volunteers.PetsOperations.DeletePets;
using PetFamily.Application.Volunteers.PetsOperations.GetPets;
using PetFamily.Application.Volunteers.Restore;
using PetFamily.Application.Volunteers.SoftDelete;
using PetFamily.Application.Volunteers.UpdateDonationsInfo;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

namespace PetFamily.Application
{
    public static class Inject
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateVolunteerHandler>();
            services.AddScoped<UpdateMainInfoHandler>();
            services.AddScoped<UpdateDonationsInfoHandler>();
            services.AddScoped<UpdateSocialNetworksHandler>();
            services.AddScoped<SoftDeleteVolunteerHandler>();
            services.AddScoped<HardDeleteVolunteerHandler>();
            services.AddScoped<RestoreVolunteerHandler>();
            services.AddScoped<AddPetHandler>();
            services.AddScoped<DeletePetHandler>();
            services.AddScoped<GetPetHandler>();

            services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

            return services;
        }
    }
}
