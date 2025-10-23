using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.DTOs.Volunteers;
using PetFamily.Domain.Shared.Entities;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Application.Volunteers.Queries.GetByIdPet
{
    public class GetPetByIdHandler : IQueryHandler<PetReadDto, GetPetByIdQuery>
    {
        private readonly IReadDbContext _readDbContext;
        private readonly ILogger<GetPetByIdHandler> _logger;
        private readonly IValidator<GetPetByIdQuery> _validator;

        public GetPetByIdHandler(
            IValidator<GetPetByIdQuery> validator,
            ILogger<GetPetByIdHandler> logger,
            IReadDbContext readDbContext)
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
