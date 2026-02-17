namespace SharedKernel.Failures
{
    public static class Errors
    {
        public static class General
        {
            public static Error ValueIsInvalid(string? name = null)
            {
                var label = name ?? "value";

                return Error.Validation("value.is.invalid", $"{label} is invalid");
            }

            public static Error NotFound(Guid? id = null)
            {
                var forId = id == null ? "" : $" for Id '{id}'";

                return Error.NotFound("record.not.found", $"record not found{forId}");
            }

            public static Error ValueIsRequired(string? name = null)
            {
                var label = name == null ? " " : " " + name + " ";

                return Error.Conflict("length.is.invalid", $"invalid{label}length");
            }

            public static Error Unexpected(string message)
            {
                return Error.Failure("server.internal", message);
            }

            public static Error FailedToSave()
            {
                return Error.Failure("data.save.failed", "failed to save data in database");
            }

            public static Error Duplicate()
            {
                return Error.Validation("record.already.exist", $"record already exist");
            }
        }

        public static class Tokens
        {
            public static Error ExpiredToken()
            {
                return Error.Unauthorized("token.is.expired", "Your token is expired");
            }
            public static Error InvalidToken()
            {
                return Error.Unauthorized("token.is.invalid", "Your token is invalid");
            }
        }

        public static class Volunteer
        {
            public static Error Duplicate()
            {
                return Error.Validation("record.already.exist", $"volunteer already exist");
            }
        }

        public static class SocialNetwork
        {
            public static Error Duplicate()
            {
                return Error.Validation("record.already.exist", $"social network already exist");
            }
        }

        public static class DonationInfo
        {
            public static Error Duplicate()
            {
                return Error.Validation("record.already.exist", $"donation information already exist");
            }
        }

        public static class SpeciesAndBreed
        {
            public static Error BreedInUse(Guid? breedId = null)
            {
                var forId = breedId == null ? "" : $" for Id '{breedId}'"; ;

                return Error.Failure("breed.in.use", $"breed in use{forId}");
            }

            public static Error SpeciesInUse(Guid? breedId = null)
            {
                var forId = breedId == null ? "" : $" for Id '{breedId}'"; ;

                return Error.Failure("species.in.use", $"species in use{forId}");
            }

            public static Error NotFound(Guid? speciesId = null, Guid? breedId = null)
            {
                var forIds = speciesId == null ? "" :
                             breedId == null ? "" :
                             $" for speciesId {speciesId} and breedId {breedId}";

                return Error.NotFound("record.not.found", $"record not found{forIds}");
            }
        }

        public static class PetFile
        {
            public static Error FileIsEmpty()
            {
                return Error.Validation("length.is.invalid", $"file is empty");
            }

            public static Error FileTooLarge()
            {
                return Error.Validation("length.is.invalid", $"file is large");
            }

            public static Error NotFound(string? name = null)
            {
                var label = name == null ? " " : " " + name + " ";

                return Error.NotFound("file.not.found", $"file{label}not found");
            }
        }

        public static class MinioProvider
        {
            public static Error FileUploadError(string? path = null, string? bucket = null)
            {
                var label = path == null ? "" : $" with objectName {path}";
                var label2 = bucket == null ? "" : $" in bucket {bucket}";

                return Error.Failure("file.upload.minio",
                    $"Fail to upload file in MinIO" + label + label2);
            }

            public static Error FileDeleteError(string? path = null, string? bucket = null)
            {
                var label = path == null ? "" : $" with objectName {path}";
                var label2 = bucket == null ? "" : $" in bucket {bucket}";

                return Error.Failure("file.delete.minio",
                    $"Fail to delete file in MinIO" + label + label2);
            }
        }

        public static class User
        {
            public static Error InvalidCredentials(string? name = null)
            {
                return Error.Validation("credentials.is.invalid", "Your credentials is invalid");
            }
        }
    }
}