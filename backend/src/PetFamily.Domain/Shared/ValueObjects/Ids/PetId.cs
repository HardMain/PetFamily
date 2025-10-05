namespace PetFamily.Domain.Aggregates.PetManagement.ValueObjects
{
    public record PetId
    {
        private PetId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static PetId NewPetId() => new PetId(Guid.NewGuid());
        public static PetId Empty() => new PetId(Guid.Empty);
        public static PetId Create(Guid id) => new(id);

        public static implicit operator Guid(PetId petId) => petId.Value;
        public static implicit operator string(PetId petId) => petId.Value.ToString();
        public static explicit operator PetId(Guid petId) => Create(petId);
    }
}