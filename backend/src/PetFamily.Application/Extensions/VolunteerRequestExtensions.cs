using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.Application.Extensions
{
    public static class VolunteerRequestExtensions
    {
        public static CreateVolunteerCommand ToCommand(this CreateVolunteerRequest request)
        {
            return new CreateVolunteerCommand(
                request.name,
                request.Email,
                request.Description,
                request.ExperienceYears,
                request.PhoneNumber,
                request.SocialNetworks ?? [],
                request.DonationsInfo ?? []
            );
        }

    }
}
