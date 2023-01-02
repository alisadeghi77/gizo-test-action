using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Infrastructure.Configurations.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gizo.Infrastructure.Configurations.UserConfig;

public class UserLocationConfig : IEntityTypeConfiguration<UserLocation>
{
    public void Configure(EntityTypeBuilder<UserLocation> builder)
    {
        builder.HasKey(model => model.Id);

        builder.ToTable("UserLocations", SchemaConfig.User);
    }
}
