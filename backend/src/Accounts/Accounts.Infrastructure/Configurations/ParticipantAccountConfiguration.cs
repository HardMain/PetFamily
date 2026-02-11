using Accounts.Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Infrastructure.Configurations
{
    public class ParticipantAccountConfiguration : IEntityTypeConfiguration<ParticipantAccount>
    {
        public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
        {
            builder.ToTable("participant_account");

            builder.HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<ParticipantAccount>(p => p.UserId);
        }
    }
}
