using Accounts.Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Infrastructure.Configurations
{
    class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
    {
        public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
        {
            builder.ToTable("volunteer_account");

            builder.HasOne(v => v.User)
                .WithOne()
                .HasForeignKey<VolunteerAccount>(v => v.UserId);
        }
    }
}
