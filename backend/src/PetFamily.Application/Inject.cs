using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.SpeciesOperations.BreedsOperations.Add;
using PetFamily.Application.SpeciesOperations.Create;
using PetFamily.Application.VolunteersOperations.Create;
using PetFamily.Application.VolunteersOperations.HardDelete;
using PetFamily.Application.VolunteersOperations.PetsOperations.Add;
using PetFamily.Application.VolunteersOperations.PetsOperations.Delete;
using PetFamily.Application.VolunteersOperations.PetsOperations.FilesOperations.AddPetFiles;
using PetFamily.Application.VolunteersOperations.PetsOperations.FilesOperations.DeletePetFiles;
using PetFamily.Application.VolunteersOperations.Restore;
using PetFamily.Application.VolunteersOperations.SoftDelete;
using PetFamily.Application.VolunteersOperations.UpdateDonationsInfo;
using PetFamily.Application.VolunteersOperations.UpdateMainInfo;
using PetFamily.Application.VolunteersOperations.UpdateSocialNetworks;

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
            services.AddScoped<AddPetFilesHandler>();
            services.AddScoped<DeletePetFilesHandler>();
            services.AddScoped<DeletePetHandler>();
            //services.AddScoped<GetPetHandler>();

            services.AddScoped<CreateSpeciesHandler>();
            services.AddScoped<AddBreedHandler>();

            services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

            return services;
        }
    }
}
