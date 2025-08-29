using PetFamily.Application.VolunteersOperations.Create;
using PetFamily.Application.VolunteersOperations.PetsOperations.Add;
using PetFamily.Application.VolunteersOperations.PetsOperations.FilesOperations.DeletePetFiles;
using PetFamily.Application.VolunteersOperations.UpdateDonationsInfo;
using PetFamily.Application.VolunteersOperations.UpdateMainInfo;
using PetFamily.Application.VolunteersOperations.UpdateSocialNetworks;
using PetFamily.Contracts.Requests.Volunteers;
using PetFamily.Contracts.Requests.Volunteers.Pets;

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
        public static DeletePetFilesCommand ToCommand(this DeletePetFilesRequest request, Guid volunteerId, Guid petId)
        {
            return new DeletePetFilesCommand(volunteerId, petId, request);
        }
        public static AddPetCommand ToCommand(this AddPetRequest request, Guid volunteerId)
        {
            return new AddPetCommand(volunteerId, request);
        }
    }
}