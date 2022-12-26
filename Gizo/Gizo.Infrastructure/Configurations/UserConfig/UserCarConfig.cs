using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Infrastructure.Configurations.BaseConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gizo.Infrastructure.Configurations.UserConfig;

public class TripTempFileConfig : IEntityTypeConfiguration<UserCar>
{
    public void Configure(EntityTypeBuilder<UserCar> builder)
    {
        //Fields
        builder.HasKey(_ => _.UserId);
        builder.HasKey(_ => _.CarModelId);

        //Relations
        builder.HasOne(_ => _.User)
               .WithMany(_ => _.UserCars)
               .HasForeignKey(_ => _.UserId);
        builder.HasOne(_ => _.CarModel)
               .WithMany(_ => _.UserCars)
               .HasForeignKey(_ => _.CarModelId);

        //Table
        builder.ToTable("UserCars", SchemaConfig.User);
    }
}
