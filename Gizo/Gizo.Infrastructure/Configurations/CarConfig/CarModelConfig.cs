using Gizo.Domain.Aggregates.CarAggregate;
using Gizo.Infrastructure.Configurations.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gizo.Infrastructure.Configurations.CarConfig;

public class CarModelConfig : IEntityTypeConfiguration<CarModel>
{
    public void Configure(EntityTypeBuilder<CarModel> builder)
    {
        //Fields
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.ModelName)
            .HasMaxLength(500)
            .IsRequired(true);

        //Relations
        builder.HasOne(_ => _.Car)
               .WithMany(_ => _.CarModels)
               .HasForeignKey(_ => _.CarId)
               .OnDelete(DeleteBehavior.Cascade);

        //Table
        builder.ToTable("CarModels", SchemaConfig.Car);
    }
}