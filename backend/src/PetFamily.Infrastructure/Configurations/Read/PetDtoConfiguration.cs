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

            builder.Property(p => p.Id)
                .HasColumnName("id");

            builder.Property(p => p.VolunteerId)
                .HasColumnName("volunteer_id");

            builder.Property(p => p.Name)
                .HasColumnName("name");

            builder.Property(p => p.Description)
                .HasColumnName("description");

            builder.OwnsOne(p => p.SpeciesAndBreed, sabB =>
            {
                sabB.Property(sab => sab.SpeciesId)
                    .HasColumnName("species_id");

                sabB.Property(sab => sab.BreedId)
                    .HasColumnName("breed_id");
            });

            builder.Property(p => p.Color)
                .HasColumnName("color");

            builder.Property(p => p.HealthInformation)
                .HasColumnName("health_information");

            builder.OwnsOne(p => p.Address, addrB =>
            {
                addrB.Property(addrB => addrB.HouseNumber)
                    .HasColumnName("house_number");

                addrB.Property(addr => addr.City)
                    .HasColumnName("city");

                addrB.Property(addr => addr.Street)
                    .HasColumnName("street");

                addrB.Property(addr => addr.Country)
                    .HasColumnName("country");
            });

            builder.Property(p => p.WeightKg)
                .HasColumnName("weight_kg");

            builder.Property(p => p.HeightCm)
                .HasColumnName("height_cm");

            builder.Property(p => p.OwnerPhone)
                .HasColumnName("owner_phone");

            builder.Property(p => p.Position)
                .HasColumnName("position");

            builder.Property(p => p.isCastrated)
                .HasColumnName("is_castrated");

            builder.Property(p => p.BirthDate)
                .HasColumnName("birth_date");

            builder.Property(p => p.isVaccinated)
                .HasColumnName("is_vaccinated");

            builder.Property(p => p.SupportStatus)
                .IsRequired()
                .HasColumnName("support_status");

            builder.OwnsMany(p => p.DonationsInfo, db =>
            {
                db.ToJson("donations_info");

                db.Property(d => d.Title);

                db.Property(d => d.Description);
            });

            builder.Property(p => p.MainPhoto)
                .HasConversion(
                pf => string.Empty,
                path => new PetFileDto { PathToStorage = path })
                .IsRequired(false)
                .HasColumnName("main_photo");

            builder.Property(p => p.Files)
                .HasConversion(
                    files => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<List<PetFileDto>>(json, JsonSerializerOptions.Default)!)
                .HasColumnName("files");
        }
    }
}