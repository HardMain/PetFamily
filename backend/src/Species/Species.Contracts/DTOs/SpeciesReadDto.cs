namespace Species.Contracts.DTOs
{
    public class SpeciesReadDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
    }
}
