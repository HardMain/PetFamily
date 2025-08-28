using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;

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

        public static implicit operator Guid(VolunteerId volunteerId) => volunteerId.Value;
        public static implicit operator string(VolunteerId volunteerId) => volunteerId.Value.ToString();
        public static explicit operator VolunteerId(Guid volunteerId) => Create(volunteerId);
    }
}
