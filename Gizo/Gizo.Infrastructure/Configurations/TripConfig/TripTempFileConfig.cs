using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Infrastructure.Configurations.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gizo.Infrastructure.Configurations.TripConfig;
public class TripTempFileConfig : IEntityTypeConfiguration<TripTempFile>
{
    public void Configure(EntityTypeBuilder<TripTempFile> builder)
    {
        // Fields
        builder.HasKey(x => x.Id);

        // Relations
        builder.HasOne(x => x.Trip)
               .WithMany(x => x.TripTempFiles)
               .HasForeignKey(x => x.TripId)
               .OnDelete(DeleteBehavior.Cascade);

        // Table
        builder.ToTable("TripTempFiles", SchemaConfig.Trip);
    }
}
