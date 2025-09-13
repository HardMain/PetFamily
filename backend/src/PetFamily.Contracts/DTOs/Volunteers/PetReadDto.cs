using PetFamily.Contracts.DTOs.Shared;
using PetFamily.Contracts.DTOs.Species;
using PetFamily.Contracts.DTOs.Volunteers.Pets;

namespace PetFamily.Contracts.DTOs.Volunteers;

public class PetReadDto
{
    public Guid Id { get; init; }
    public Guid VolunteerId { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public SpeciesAndBreedDto SpeciesAndBreed { get; init; } = default!;
    public string Color { get; init; } = default!;
    public string HealthInformation { get; init; } = default!;
    public AddressDto Address { get; init; } = default!;
    public double WeightKg { get; init; }
    public double HeightCm { get; init; }
    public string NumberPhone { get; init; } = default!;
    public bool IsCastrated { get; init; }
    public DateTime BirthDate { get; init; }
    public bool IsVaccinated { get; init; }
    public string SupportStatus { get; init; } = default!;
    //public string[] DonationsInfo { get; init; } = [];
    public PetFileDto[] Files { get; init; } = [];
}
