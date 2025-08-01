using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Aggregates.PetManagement.Entities;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using FluentValidation;
using PetFamily.Application.Extensions;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteersHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly IValidator<CreateVolunteerCommand> _validator;

        public CreateVolunteersHandler(
            IVolunteersRepository volunteersRepository,
            IValidator<CreateVolunteerCommand> validator)
        {
            _volunteersRepository = volunteersRepository;
            _validator = validator;
        }

        public async Task<Result<Guid, ErrorList>> Handle(CreateVolunteerCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return validationResult.ToErrorList();

            var phoneNumber = PhoneNumber.Create(command.Request.PhoneNumber).Value;
           
            var volunteerByPhone = await _volunteersRepository.GetByPhoneNumber(phoneNumber, cancellationToken);
            if (volunteerByPhone.IsSuccess)
                return Errors.Volunteer.AlreadyExist().ToErrorList();

            var email = Email.Create(command.Request.Email).Value;

            var volunteerByEmail = await _volunteersRepository.GetByEmail(email, cancellationToken);
            if (volunteerByEmail.IsSuccess)
                return Errors.Volunteer.AlreadyExist().ToErrorList();

            var name = FullName.Create(
                command.Request.FullName.firstName, 
                command.Request.FullName.lastName, 
                command.Request.FullName.middleName).Value;
               
            var description = command.Request.Description;

            var experienceYears = command.Request.ExperienceYears;

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
                return volunteerResult.Error.ToErrorList();

            await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);

            return volunteerResult.Value.Id.Value; 
        }
    }
}