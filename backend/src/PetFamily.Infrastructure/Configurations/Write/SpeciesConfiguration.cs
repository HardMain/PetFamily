using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Aggregates.Species.Entities;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Infrastructure.Configurations.Write
{
    public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
    {
        public void Configure(EntityTypeBuilder<Species> builder)
        {
            builder.ToTable("species");

            builder.HasKey(s => s.Id)
                .HasName("pk_species");

            builder.Property(s => s.Id)
                .IsRequired()
                .HasConversion(
                    id => id.Value,
                    value => SpeciesId.Create(value)
                )
                .HasColumnName("id");

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");

            builder.HasMany(s => s.Breeds)
                .WithOne()
                .HasForeignKey("species_id")
                .HasConstraintName("fk_breeds_species_species_id")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
