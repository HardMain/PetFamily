using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.SpeciesAggregate.Entities;

namespace PetFamily.Infrastructure.Configurations.Write
{
    public class BreedConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder)
        {
            builder.ToTable("breeds");

            builder.HasKey(b => b.Id)
                .HasName("pk_breeds");

            builder.Property(b => b.Id)
                .IsRequired()
                .HasConversion(
                    id => id.Value,
                    value => BreedId.Create(value))
                .HasColumnName("id");

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");
        }
    }
}
