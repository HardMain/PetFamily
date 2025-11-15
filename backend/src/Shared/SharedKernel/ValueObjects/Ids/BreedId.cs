namespace SharedKernel.ValueObjects.Ids
{
    public record BreedId
    {
        private BreedId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static BreedId NewBreedId() => new BreedId(Guid.NewGuid());
        public static BreedId Empty() => new BreedId(Guid.Empty);
        public static BreedId Create(Guid id) => new(id);

        public static implicit operator Guid(BreedId breedId) => breedId.Value;
        public static implicit operator string(BreedId breedId) => breedId.Value.ToString();
        public static explicit operator BreedId(Guid breedId) => Create(breedId);
    }
}