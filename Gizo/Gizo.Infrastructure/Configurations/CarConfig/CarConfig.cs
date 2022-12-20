using Gizo.Domain.Aggregates.CarAggregate;
using Gizo.Infrastructure.Configurations.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gizo.Infrastructure.Configurations.CarConfig;

public class CarConfig : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        //Fields
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.CarName)
            .HasMaxLength(500)
            .IsRequired(true);
        builder.Property(_ => _.IsAvailable)
            .HasDefaultValue(true);

        //Table
        builder.ToTable("Cars", SchemaConfig.Car);
    }
}