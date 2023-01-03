using Gizo.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gizo.Infrastructure.Configurations;

public class UserVerificationTokenConfiguration : IEntityTypeConfiguration<UserVerificationCode>
{
    public void Configure(EntityTypeBuilder<UserVerificationCode> builder)
    {
        builder.HasOne<User>()
            .WithMany(_ => _.UserVerificationCodes)
            .HasForeignKey(_ => _.UserId);

        builder.Property(model => model.Code)
            .IsRequired();

        builder.ToTable("UserVerificationCodes");
    }
}
