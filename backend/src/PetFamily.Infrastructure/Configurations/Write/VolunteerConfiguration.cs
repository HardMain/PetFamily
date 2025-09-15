using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared.Constants;
using PetFamily.Domain.Aggregates.PetManagement.Entities;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Infrastructure.Configurations.Write
{
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable("volunteers").
                HasQueryFilter(v => !v.IsDeleted);

            builder.HasKey(v => v.Id)
                .HasName("pk_volunteers");

            builder.Property(v => v.Id)
                .HasConversion(
                    id => id.Value,
                    value => VolunteerId.Create(value)
                )
                .HasColumnName("id");

            builder.OwnsOne(v => v.Name, nb =>
            {
                nb.Property(n => n.FirstName)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("first_name");

                nb.Property(n => n.LastName)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("last_name");

                nb.Property(n => n.MiddleName)
                    .IsRequired(false)
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("middle_name");
            });

            builder.OwnsOne(v => v.Email, emb =>
            {
                emb.Property(em => em.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("email");
            });

            builder.Property(v => v.Description)
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .HasColumnName("description");

            builder.Property(v => v.ExperienceYears)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("experience_years");

            builder.OwnsOne(v => v.Number, phb =>
            {
                phb.Property(ph => ph.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("phone_number");
            });

            builder.OwnsMany(v => v.SocialNetworks, sb =>
            {
                sb.ToJson("socials");

                sb.Property(s => s.URL)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                    .HasColumnName("url");

                sb.Property(s => s.Platform)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("platform");
            });

            builder.OwnsMany(v => v.DonationsInfo, db =>
            {
                db.ToJson("dontations_info");

                db.Property(d => d.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .HasColumnName("title");

                db.Property(d => d.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                    .HasColumnName("description");
            });

            builder.HasMany(v => v.Pets)
                .WithOne()
                .HasForeignKey("volunteer_id")
                .HasConstraintName("fk_pets_volunteers_volunteer_id")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(v => v.IsDeleted)
                .IsRequired()
                .HasColumnName("is_deleted");

            builder.Property(v => v.DeletionDate)
                .IsRequired(false)
                .HasColumnName("deletion_date");
        }
    }
}
