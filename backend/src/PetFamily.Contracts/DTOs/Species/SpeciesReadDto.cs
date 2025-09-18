namespace PetFamily.Contracts.DTOs.Species
{
    public class SpeciesReadDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
    }
}
