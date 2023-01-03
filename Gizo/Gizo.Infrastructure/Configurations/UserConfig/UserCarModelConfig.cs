using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Infrastructure.Configurations.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gizo.Infrastructure.Configurations.UserConfig;

public class TripTempFileConfig : IEntityTypeConfiguration<UserCarModel>
{
    public void Configure(EntityTypeBuilder<UserCarModel> builder)
    {
        // Fields
        builder.HasKey(_ => _.Id);

        // Relations
        builder.HasOne(_ => _.User)
               .WithMany(_ => _.UserCarModels)
               .HasForeignKey(_ => _.UserId);
        builder.HasOne(_ => _.CarModel)
               .WithMany(_ => _.UserCarModels)
               .HasForeignKey(_ => _.CarModelId);

        // Table
        builder.ToTable("UserCarModels", SchemaConfig.User);
    }
}
