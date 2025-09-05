using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Aggregates.PetManagement.Entities;
using PetFamily.Domain.Aggregates.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Constants;

namespace PetFamily.Infrastructure.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("pets")
                .HasQueryFilter(v => !v.IsDeleted);

            builder.HasKey(p => p.Id)
                .HasName("pk_pets");

            builder.Property(p => p.Id)
                .HasConversion(
                    id => id.Value,
                    value => PetId.Create(value)
                )
                .HasColumnName("id");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .HasColumnName("description");

            builder.OwnsOne(p => p.SpeciesAndBreed, sbb =>
            {
                sbb.OwnsOne(sb => sb.SpeciesId, idb =>
                {
                    idb.Property(sid => sid.Value)
                        .IsRequired()
                        .HasColumnName("species_id");
                });

                sbb.OwnsOne(sb => sb.BreedId, idb =>
                {
                    idb.Property(bid => bid.Value)
                        .IsRequired()
                        .HasColumnName("breed_id");
                });
            });

            builder.Property(p => p.Color)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("color");

            builder.Property(p => p.HealthInformation)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                .HasColumnName("health_information");

            builder.OwnsOne(p => p.Address, adb =>
            {
                adb.Property(ad => ad.Street)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("street");

                adb.Property(ad => ad.City)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("city");

                adb.Property(ad => ad.Country)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("country");

                adb.Property(ad => ad.HouseNumber)
                    .IsRequired()
                    .HasColumnName("house_number");
            });

            builder.Property(p => p.WeightKg)
                .IsRequired()
                .HasColumnName("weight_kg");

            builder.Property(p => p.HeightCm)
                .IsRequired()
                .HasColumnName("height_cm");

            builder.OwnsOne(p => p.OwnerPhone, phb =>
            {
                phb.Property(ph => ph.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("owner_phone");
            });

            builder.OwnsOne(p => p.Position, sn =>
            {
                sn.Property(sn => sn.Value)
                    .IsRequired()
                    .HasColumnName("position");
            });

            builder.Property(p => p.isCastrated)
                .IsRequired()
                .HasColumnName("is_castrated");

            builder.Property(p => p.BirthDate)
                .IsRequired()
                .HasColumnName("birth_date");

            builder.Property(p => p.isVaccinated)
                .IsRequired()
                .HasColumnName("is_vaccinated");

            builder.Property(p => p.SupportStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("support_status");

            builder.OwnsMany(p => p.DonationsInfo, db =>
            {
                db.ToJson("donations_info");

                db.Property(d => d.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                db.Property(d => d.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
            });

            builder.OwnsMany(p => p.Files, pfb =>
            {
                pfb.ToJson("files");

                pfb.OwnsOne(pf => pf.PathToStorage, fpb =>
                {
                    fpb.Property(fp => fp.Path)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
                });
            });

            builder.Property(p => p.CreationDate)
                .IsRequired()
                .HasColumnName("creation_date");

            builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasColumnName("is_deleted");

            builder.Property(p => p.DeletionDate)
                .IsRequired(false)
                .HasColumnName("deletion_date");
        }
    }
}
