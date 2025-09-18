using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Contracts.DTOs.Volunteers;
using PetFamily.Contracts.DTOs.Volunteers.Pets;

namespace PetFamily.Infrastructure.Configurations.Read
{
    public class PetDtoConfiguration : IEntityTypeConfiguration<PetReadDto>
    {
        public void Configure(EntityTypeBuilder<PetReadDto> builder)
        {
            builder.ToTable("pets");

            builder.HasKey(p => p.Id);

            builder.Property(v => v.Id)
                .HasColumnName("id");

            builder.OwnsOne(p => p.SpeciesAndBreed, sabB =>
            {
                sabB.Property(sab => sab.SpeciesId)
                    .HasColumnName("species_id");

                sabB.Property(sab => sab.BreedId)
                    .HasColumnName("breed_id");
            });

            builder.OwnsOne(p => p.Address, addrB =>
            {
                addrB.Property(addr => addr.City)
                    .HasColumnName("city");

                addrB.Property(addr => addr.Street)
                    .HasColumnName("street");

                addrB.Property(addr => addr.Country)
                    .HasColumnName("country");
            });

            builder.Property(p => p.Files)
                .HasConversion(
                    files => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<PetFileDto[]>(json, JsonSerializerOptions.Default)!);
        }
    }
}
