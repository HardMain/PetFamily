using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Domain.VolunteersAggregate.ValueObjects
{
    public record FilePath
    {
        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".mp4",
            ".avi",
            ".mkv",
            ".mov"
        };

        private FilePath(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public static Result<FilePath> Create(Guid path, string extension)
        {
            if (string.IsNullOrWhiteSpace(extension) && !AllowedExtensions.Contains(extension))
                return Errors.General.ValueIsInvalid("extension");

            var fullPath = path + extension;

            return new FilePath(fullPath);
        }

        public static Result<FilePath> Create(string fullPath)
        {
            var extension = System.IO.Path.GetExtension(fullPath);

            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
                return Errors.General.ValueIsInvalid("extension");

            return new FilePath(fullPath);
        }
    }
}