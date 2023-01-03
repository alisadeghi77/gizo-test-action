using Gizo.Domain.Aggregates.CarBrandAggregate;
using Gizo.Infrastructure.Configurations.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gizo.Infrastructure.Configurations.CarConfig;

public class CarModelConfig : IEntityTypeConfiguration<CarModel>
{
    public void Configure(EntityTypeBuilder<CarModel> builder)
    {
        // Fields
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Name)
            .HasMaxLength(500)
            .IsRequired();

        // Relations
        builder.HasOne(_ => _.CarBrand)
               .WithMany(_ => _.CarModels)
               .HasForeignKey(_ => _.CarBrandId)
               .OnDelete(DeleteBehavior.Cascade);

        // Table
        builder.ToTable("CarModels", SchemaConfig.CarBrand);
    }
}
