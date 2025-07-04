namespace PetFamily.Domain.ValueObjects
{
    public record VolunteerId
    {
        private VolunteerId(Guid value)
        {
            Value = value;
        }
        public Guid Value { get; }

        public static VolunteerId NewPetId() => new VolunteerId(Guid.NewGuid());

        public static VolunteerId Empty() => new VolunteerId(Guid.Empty);
        public static VolunteerId Create(Guid id) => new(id);
    }
}
