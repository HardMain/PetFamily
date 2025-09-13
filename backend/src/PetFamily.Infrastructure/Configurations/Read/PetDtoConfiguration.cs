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

            builder.OwnsOne(p => p.SpeciesAndBreed, sabB =>
            {
                sabB.Property(sab => sab.SpeciesId);
                sabB.Property(sab => sab.BreedId);
            });

            builder.OwnsOne(p => p.Address, addrB =>
            {
                addrB.Property(addr => addr.City);
                addrB.Property(addr => addr.Street);
                addrB.Property(addr => addr.Country);
            });

            builder.Property(p => p.Files)
                .HasConversion(
                    files => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<PetFileDto[]>(json, JsonSerializerOptions.Default)!);
        }
    }
}
