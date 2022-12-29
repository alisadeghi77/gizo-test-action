using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Infrastructure.Configurations.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gizo.Infrastructure.Configurations.TripConfig;

public class TripConfig : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        //Fields
        builder.HasKey(x => x.Id);

        //Relations
        builder.HasOne(x => x.User)
               .WithMany(x => x.Trips)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.UserCarModel)
               .WithMany(x => x.Trips)
               .HasForeignKey(x => x.UserCarModelId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);

        //Table
        builder.ToTable("Trips", SchemaConfig.Trip);
    }
}
