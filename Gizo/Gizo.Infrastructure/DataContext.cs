using Gizo.Domain.Aggregates.CarBrandAggregate;
using Gizo.Domain.Aggregates.RoleAggregate;
using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Infrastructure;

public class DataContext : IdentityDbContext<User, Role, long>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Trip> Trips { get; set; } = null!;

    public DbSet<CarBrand> Cars { get; set; } = null!;

    public DbSet<UserLocation> UserLocations { get; set; } = null!;

    public override int SaveChanges()
    {
        try
        {
            var entityID = base.SaveChanges();

            DetachAll();
            return entityID;
        }
        catch (Exception)
        {
            DetachAll();
            throw;
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var entityID = await base.SaveChangesAsync(cancellationToken);

            DetachAll();
            return entityID;
        }
        catch (Exception)
        {
            DetachAll();
            throw;
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyAllConfigurations();
        base.OnModelCreating(builder);
    }

    private void DetachAll()
    {
        ChangeTracker.Clear();
    }
}
