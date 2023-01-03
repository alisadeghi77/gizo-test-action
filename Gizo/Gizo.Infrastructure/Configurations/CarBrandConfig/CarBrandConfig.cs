using Gizo.Domain.Aggregates.CarBrandAggregate;
using Gizo.Infrastructure.Configurations.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gizo.Infrastructure.Configurations.CarBrandConfig;

public class CarBrandConfig : IEntityTypeConfiguration<CarBrand>
{
    public void Configure(EntityTypeBuilder<CarBrand> builder)
    {
        // Fields
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Name)
            .HasMaxLength(500)
            .IsRequired(true);
        builder.Property(_ => _.IsAvailable)
            .HasDefaultValue(true);

        // Table
        builder.ToTable("CarBrands", SchemaConfig.CarBrand);
    }
}
