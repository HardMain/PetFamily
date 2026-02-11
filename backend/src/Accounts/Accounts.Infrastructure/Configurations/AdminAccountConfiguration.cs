using Accounts.Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Infrastructure.Configurations
{
    public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
    {
        public void Configure(EntityTypeBuilder<AdminAccount> builder)
        {
            builder.ToTable("admin_account");

            builder.HasOne(a => a.User)
               .WithOne()
               .HasForeignKey<AdminAccount>(a => a.UserId);

            builder.OwnsOne(a => a.FullName, fb =>
            {
                fb.Property(f => f.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name");

                fb.Property(f => f.LastName)
                    .IsRequired()
                    .HasColumnName("last_name");

                fb.Property(f => f.MiddleName)
                    .HasColumnName("middle_name");
            });
        }
    }
}
