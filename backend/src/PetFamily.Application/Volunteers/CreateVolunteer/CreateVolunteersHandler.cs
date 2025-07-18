using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.ValueObjects;
using PetFamily.Domain.Volunteers.Entities;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteersHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;

        public CreateVolunteersHandler(IVolunteersRepository volunteersRepository)
        {
            _volunteersRepository = volunteersRepository;
        }

        public async Task<Result<Guid, Error>> Handle(CreateVolunteerCommand command, CancellationToken cancellationToken = default)
        {
            var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber);
            if (phoneNumberResult.IsFailure)
                return phoneNumberResult.Error;

            var emailResult = Email.Create(command.Email);
            if (emailResult.IsFailure)
                return emailResult.Error;

            var volunteerByPhone = await _volunteersRepository.GetByPhoneNumber(phoneNumberResult.Value.Value);
            if (volunteerByPhone.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            var volunteerByEmail = await _volunteersRepository.GetByEmail(emailResult.Value.Value);
            if (volunteerByEmail.IsSuccess)
                return Errors.Volunteer.AlreadyExist();

            var volunteerId = VolunteerId.NewVolunteerId(); 

            var nameResult = FullName.Create(command.FullName.firstName, command.FullName.lastName, command.FullName.middleName);
            if (nameResult.IsFailure)
                return nameResult.Error;
               
            var description = command.Description;

            var experienceYears = command.ExperienceYears;

            var volunteerResult = Volunteer.Create(
                volunteerId, 
                nameResult.Value, 
                emailResult.Value, 
                description, 
                experienceYears, 
                phoneNumberResult.Value
            );

            if (volunteerResult.IsFailure) 
                return volunteerResult.Error;

            await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);

            return volunteerResult.Value.Id.Value; 
        }

    }
}
