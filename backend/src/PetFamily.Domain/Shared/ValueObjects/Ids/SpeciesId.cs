using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;

namespace PetFamily.Domain.Shared.ValueObjects.Ids
{
    public record SpeciesId
    {
        private SpeciesId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static SpeciesId NewSpeciesId() => new SpeciesId(Guid.NewGuid());
        public static SpeciesId Empty() => new SpeciesId(Guid.Empty);
        public static SpeciesId Create(Guid id) => new(id);

        public static implicit operator Guid(SpeciesId speciesId) => speciesId.Value;
        public static implicit operator string(SpeciesId speciesId) => speciesId.Value.ToString();
        public static explicit operator SpeciesId(Guid speciesId) => Create(speciesId);
    }
}
