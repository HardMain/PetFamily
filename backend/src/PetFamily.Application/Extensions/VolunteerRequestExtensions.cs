using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Contracts.Requests.Volunteers;

namespace PetFamily.Application.Extensions
{
    public static class VolunteerRequestExtensions
    {
        public static CreateVolunteerCommand ToCommand(this CreateVolunteerRequest request)
        {
            return new CreateVolunteerCommand(request);
        }
    }
}