namespace PetFamily.Domain.Shared.ValueObjects.Ids
{
    public record VolunteerId
    {
        private VolunteerId(Guid value)
        {
            Value = value;
        }
        public Guid Value { get; }

        public static VolunteerId NewVolunteerId() => new VolunteerId(Guid.NewGuid());

        public static VolunteerId Empty() => new VolunteerId(Guid.Empty);
        public static VolunteerId Create(Guid id) => new(id);

        public static implicit operator Guid(VolunteerId volunteerId)
        {
            if (volunteerId is null)
                throw new ArgumentNullException();

            return volunteerId.Value;
        }
    }
}
