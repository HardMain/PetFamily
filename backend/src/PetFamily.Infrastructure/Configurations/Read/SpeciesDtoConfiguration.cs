using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Contracts.SpeciesAggregate.DTOs;

namespace PetFamily.Infrastructure.Configurations.Read
{
    public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesReadDto>
    {
        public void Configure(EntityTypeBuilder<SpeciesReadDto> builder)
        {
            builder.ToTable("species");

            builder.HasKey(s => s.Id);

            builder.Property(v => v.Id)
                .HasColumnName("id");

            builder.Property(s => s.Name)
                .HasColumnName("name");
        }
    }
}
