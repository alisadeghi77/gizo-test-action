using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Infrastructure.Configurations.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gizo.Infrastructure.Configurations.TripConfig;
public class TripTempVideoConfig : IEntityTypeConfiguration<TripTempVideo>
{
    public void Configure(EntityTypeBuilder<TripTempVideo> builder)
    {
        //Fields
        builder.HasKey(x => x.Id);

        //Relations
        builder.HasOne(x => x.Trip)
               .WithMany(x => x.TripTempVideos)
               .HasForeignKey(x => x.TripId)
               .OnDelete(DeleteBehavior.Cascade);

        //Table
        builder.ToTable("TripTempVideos", SchemaConfig.Trip);
    }
}
