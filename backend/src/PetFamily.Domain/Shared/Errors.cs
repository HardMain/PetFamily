using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Shared
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

                return Error.NotFound("record.not.fountd", $"record not found{forId}");
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
    }
}
