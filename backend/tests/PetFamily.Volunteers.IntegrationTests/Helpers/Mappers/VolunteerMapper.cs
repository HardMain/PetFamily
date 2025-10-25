using PetFamily.Contracts.VolunteersAggregate.DTOs;
using PetFamily.Contracts.VolunteersAggregate.Requests;

namespace PetFamily.Volunteers.IntegrationTests.Helpers.Mappers
{
    public class VolunteerMapper
    {
        public static UpdateMainInfoRequest ToUpdateRequest(VolunteerReadDto volunteer)
        {
            return new UpdateMainInfoRequest(
                volunteer.FullName,
                volunteer.Email,
                volunteer.PhoneNumber,
                volunteer.Description,
                volunteer.ExperienceYears
                );
        }
    }
}
