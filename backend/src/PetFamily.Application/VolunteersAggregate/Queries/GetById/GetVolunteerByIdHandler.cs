using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.DTOs.Volunteers;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Application.VolunteersAggregate.Queries.GetById
{
    public class GetVolunteerByIdHandler : IQueryHandler<VolunteerReadDto, GetVolunteerByIdQuery>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly ILogger<GetVolunteerByIdHandler> _logger;
        private readonly IValidator<GetVolunteerByIdQuery> _validator;

        public GetVolunteerByIdHandler(
            IValidator<GetVolunteerByIdQuery> validator,
            ILogger<GetVolunteerByIdHandler> logger,
            IReadDbContext readDbContext)
        {
            _validator = validator;
            _logger = logger;
            _readDbContext = readDbContext;
        }

        public async Task<Result<VolunteerReadDto, ErrorList>> Handle(
            GetVolunteerByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var volunteer = await _readDbContext.Volunteers.FirstOrDefaultAsync(v => v.Id == query.Id, cancellationToken);
            if (volunteer == null)
            {
                _logger.LogWarning("Failed to get volunteer {VolunteerId}", query.Id);

                return Errors.General.NotFound(query.Id).ToErrorList();
            }

            return volunteer;
        }
    }
}
