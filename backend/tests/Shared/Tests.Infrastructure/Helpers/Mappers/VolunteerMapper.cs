using Volunteers.Contracts.DTOs;
using Volunteers.Contracts.Requests;

namespace Tests.Infrastructure.Helpers.Mappers
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
