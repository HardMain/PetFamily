namespace SharedKernel.Constants
{
    public static class Permissions
    {
        public static class Pets
        {
            public const string Read = "pets.read";
            public const string Create = "pets.create";
            public const string Update = "pets.update";
            public const string Delete = "pets.delete";
            public const string Restore = "pets.restore";
            public const string ManageFiles = "pets.manage_files";
        }

        public static class Volunteers
        {
            public const string Read = "volunteers.read";
            public const string Create = "volunteers.create";
            public const string Update = "volunteers.update";
            public const string Delete = "volunteers.delete";
            public const string Restore = "volunteers.restore";
        }

        public static class Species
        {
            public const string Read = "species.read";
            public const string Create = "species.create";
            public const string Delete = "species.delete";
        }

        public static class Breeds
        {
            public const string Read = "breeds.read";
            public const string Create = "breeds.create";
            public const string Delete = "breeds.delete";
        }
    }
}
