using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Contracts.DTOs.Volunteers;

namespace PetFamily.Infrastructure.Configurations.Read
{
    public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerReadDto>
    {
        public void Configure(EntityTypeBuilder<VolunteerReadDto> builder)
        {
            builder.ToTable("volunteers");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasColumnName("id");

            builder.OwnsOne(v => v.FullName, fnB =>
            {
                fnB.Property(fn => fn.FirstName)
                    .HasColumnName("first_name");

                fnB.Property(fn => fn.MiddleName)
                    .HasColumnName("middle_name");

                fnB.Property(fn => fn.LastName)
                    .HasColumnName("last_name");
            });

            builder.Property(v => v.Email)
                .HasColumnName("email");

            builder.Property(v => v.Description)
                .HasColumnName("description");

            builder.Property(v => v.ExperienceYears)
                .HasColumnName("experience_years");

            builder.Property(v => v.PhoneNumber)
                .HasColumnName("phone_number");

            builder.OwnsMany(v => v.SocialNetworks, sb =>
            {
                sb.ToJson("socials");

                sb.Property(s => s.URL)
                    .HasColumnName("url");

                sb.Property(s => s.Platform)
                    .HasColumnName("platform");
            });

            builder.OwnsMany(v => v.DonationsInfo, db =>
            {
                db.ToJson("donations_info");

                db.Property(d => d.Title)
                    .HasColumnName("title");

                db.Property(d => d.Description)
                    .HasColumnName("description");
            });
        }
    }
}