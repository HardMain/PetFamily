using Core.Dtos;

namespace Volunteers.Contracts.DTOs;

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
    public string OwnerPhone { get; init; } = default!;
    public bool isCastrated { get; init; }
    public DateTime BirthDate { get; init; }
    public bool isVaccinated { get; init; }
    public string SupportStatus { get; init; } = default!;
    public PetFileDto MainPhoto { get; init; } = default!;
    public List<DonationInfoDto> DonationsInfo { get; init; } = [];
    public List<PetFileDto> Files { get; init; } = [];
    public int Position { get; init; }
}