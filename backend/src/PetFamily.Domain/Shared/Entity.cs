namespace PetFamily.Domain.Shared
{
    public abstract class Entity<Tid>
    {
        protected Entity(Tid id)
        {
            Id = id;
        }

        public Tid Id { get; private set; }
    }
}
