using FluentValidation;
using Framework.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Abstractions;
using SharedKernel.Failures;
using Volunteers.Application.Abstractions;
using Volunteers.Contracts.DTOs;

namespace Volunteers.Application.Queries.GetById
{
    public class GetVolunteerByIdHandler : IQueryHandler<VolunteerReadDto, GetVolunteerByIdQuery>
    {
        private readonly IVolunteersReadDbContext _readDbContext;
        private readonly ILogger<GetVolunteerByIdHandler> _logger;
        private readonly IValidator<GetVolunteerByIdQuery> _validator;

        public GetVolunteerByIdHandler(
            IValidator<GetVolunteerByIdQuery> validator,
            ILogger<GetVolunteerByIdHandler> logger,
            IVolunteersReadDbContext readDbContext)
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
