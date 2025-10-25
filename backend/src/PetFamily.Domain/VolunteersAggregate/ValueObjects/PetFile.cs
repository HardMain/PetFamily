namespace PetFamily.Domain.VolunteersAggregate.ValueObjects
{
    public record PetFile
    {
        private PetFile() { }
        public PetFile(FilePath pathToStorage)
        {
            PathToStorage = pathToStorage;
        }

        public FilePath PathToStorage { get; }
    }
}