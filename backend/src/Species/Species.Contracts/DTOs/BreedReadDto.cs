namespace Species.Contracts.DTOs
{
    public class BreedReadDto
    {
        public Guid Id { get; init; }
        public Guid SpeciesId { get; init; }
        public string Name { get; init; } = default!;
    }
}