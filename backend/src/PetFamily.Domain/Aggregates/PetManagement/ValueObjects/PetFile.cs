namespace PetFamily.Domain.Aggregates.PetManagement.ValueObjects
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