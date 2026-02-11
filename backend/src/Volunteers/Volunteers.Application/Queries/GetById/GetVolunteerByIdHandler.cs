using Core.Abstractions;
using Core.Caching;
using Core.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SharedKernel.Failures;
using Volunteers.Application.Abstractions;
using Volunteers.Contracts.DTOs;

namespace Volunteers.Application.Queries.GetById
{
    public class GetVolunteerByIdHandler : IQueryHandler<VolunteerReadDto, GetVolunteerByIdQuery>
    {
        private readonly DistributedCacheEntryOptions _cacheOptions = new()
        {
            SlidingExpiration = TimeSpan.FromMinutes(5),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };

        private readonly IVolunteersReadDbContext _readDbContext;
        private readonly ILogger<GetVolunteerByIdHandler> _logger;
        private readonly IValidator<GetVolunteerByIdQuery> _validator;
        private readonly ICacheService _cache;
        public GetVolunteerByIdHandler(
            IValidator<GetVolunteerByIdQuery> validator,
            ILogger<GetVolunteerByIdHandler> logger,
            IVolunteersReadDbContext readDbContext,
            ICacheService cache)
        {
            _validator = validator;
            _logger = logger;
            _readDbContext = readDbContext;
            _cache = cache;
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

            string key = "volunteer:" + query.Id;

            var volunteer = await _cache.GetOrSetAsync(
                key,
                _cacheOptions,
                async () =>
                {
                    return await _readDbContext.Volunteers
                        .FirstOrDefaultAsync(v => v.Id == query.Id, cancellationToken);
                },
                cancellationToken);

            if (volunteer == null)
            {
                _logger.LogWarning("Failed to get volunteer {VolunteerId}", query.Id);

                return Errors.General.NotFound(query.Id).ToErrorList();
            }

            return volunteer;
        }
    }
}
