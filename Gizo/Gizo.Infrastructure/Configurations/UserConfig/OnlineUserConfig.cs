using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Infrastructure.Configurations.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gizo.Infrastructure.Configurations.UserConfig;

public class OnlineUserConfig : IEntityTypeConfiguration<OnlineUser>
{
    public void Configure(EntityTypeBuilder<OnlineUser> builder)
    {
        //Fields
        builder.HasKey(x => x.Id);

        //Table
        builder.ToTable("OnlineUsers", SchemaConfig.User);
    }
}