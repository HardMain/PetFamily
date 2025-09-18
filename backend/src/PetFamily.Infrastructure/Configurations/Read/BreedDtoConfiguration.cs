using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Configurations.Read
{
    public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedReadDto>
    {
        public void Configure(EntityTypeBuilder<BreedReadDto> builder)
        {
            builder.ToTable("breeds");

            builder.HasKey(b => b.Id);

            builder.Property(v => v.Id)
                .HasColumnName("id");

            builder.Property(b => b.Name)
                .HasColumnName("name");

            builder.Property(b => b.SpeciesId)
                .HasColumnName("species_id");
        }
    }
}
