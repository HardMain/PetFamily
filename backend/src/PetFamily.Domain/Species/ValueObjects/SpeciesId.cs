namespace PetFamily.Domain.Species.ValueObjects
{
    public record SpeciesId
    {
        private SpeciesId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static SpeciesId NewPetId() => new SpeciesId(Guid.NewGuid());
        public static SpeciesId Empty() => new SpeciesId(Guid.Empty);
        public static SpeciesId Create(Guid id) => new(id);
    }
}
