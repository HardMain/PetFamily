namespace PetFamily.Domain.Volunteers.ValueObjects
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
    }
}