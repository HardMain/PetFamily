using FluentValidation;
using Framework.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Abstractions;
using SharedKernel.Failures;
using Volunteers.Application.Abstractions;
using Volunteers.Contracts.DTOs;

namespace Volunteers.Application.Queries.GetByIdPet
{
    public class GetPetByIdHandler : IQueryHandler<PetReadDto, GetPetByIdQuery>
    {
        private readonly IVolunteersReadDbContext _readDbContext;
        private readonly ILogger<GetPetByIdHandler> _logger;
        private readonly IValidator<GetPetByIdQuery> _validator;

        public GetPetByIdHandler(
            IValidator<GetPetByIdQuery> validator,
            ILogger<GetPetByIdHandler> logger,
            IVolunteersReadDbContext readDbContext)
        {
            _validator = validator;
            _logger = logger;
            _readDbContext = readDbContext;
        }

        public async Task<Result<PetReadDto, ErrorList>> Handle(
            GetPetByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(query, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed: {Errors}", validationResult.ToErrorList());

                return validationResult.ToErrorList();
            }

            var pet = await _readDbContext.Pets.FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);
            if (pet == null)
            {
                _logger.LogWarning("Failed to get pet {PetId}", query.Id);

                return Errors.General.NotFound(query.Id).ToErrorList();
            }

            return pet;
        }
    }
}
