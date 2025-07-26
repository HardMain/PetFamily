using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Aggregates.PetManagement.Entities;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteersHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;

        public CreateVolunteersHandler(IVolunteersRepository volunteersRepository)
        {
            _volunteersRepository = volunteersRepository;
        }

        public async Task<Result<Guid>> Handle(CreateVolunteerCommand command, CancellationToken cancellationToken = default)
        {
            var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
           
            var volunteerByPhone = await _volunteersRepository.GetByPhoneNumber(phoneNumber, cancellationToken);
            if (volunteerByPhone.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            var email = Email.Create(command.Email).Value;

            var volunteerByEmail = await _volunteersRepository.GetByEmail(email, cancellationToken);
            if (volunteerByEmail.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            var name = FullName.Create(command.FullName.firstName, command.FullName.lastName, command.FullName.middleName).Value;
               
            var description = command.Description;

            var experienceYears = command.ExperienceYears;

            var volunteerId = VolunteerId.NewVolunteerId(); 

            var volunteerResult = Volunteer.Create(
                volunteerId, 
                name, 
                email, 
                description, 
                experienceYears, 
                phoneNumber
            );

            if (volunteerResult.IsFailure) 
                return volunteerResult.Error;

            await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);

            return volunteerResult.Value.Id.Value; 
        }

    }
}