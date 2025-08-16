using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.PetsOperations.AddPets;
using PetFamily.Application.Volunteers.UpdateDonationsInfo;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;
using PetFamily.Contracts.DTOs.Shared;
using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.Extensions
{
    public static class VolunteerRequestExtensions
    {
        public static CreateVolunteerCommand ToCommand(this CreateVolunteerRequest request)
        {
            return new CreateVolunteerCommand(request);
        }
        public static UpdateMainInfoCommand ToCommand(this UpdateMainInfoRequest request, Guid id)
        {
            return new UpdateMainInfoCommand(id, request);
        }
        public static UpdateSocialNetworksCommand ToCommand(this UpdateSocialNetworksRequest request, Guid id)
        {
            return new UpdateSocialNetworksCommand(id, request);
        }
        public static UpdateDonationsInfoCommand ToCommand(this UpdateDonationsInfoRequest request, Guid id)
        {
            return new UpdateDonationsInfoCommand(id, request);
        }
        //public static AddPetCommand ToCommand(this AddPetRequest request, FileMetadataDTO fileMetadataDTO)
        //{
        //    return new AddPetCommand(request, fileMetadataDTO);
        //}
    }
}