using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Domain.Aggregates.RoleAggregate;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Domain.Contracts.Repository;
using Gizo.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Infrastructure;

public class DataContext : IdentityDbContext<User, Role, long>, IUnitOfWork
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyAllConfigurations();
        base.OnModelCreating(modelBuilder);
    }

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

    public override void Dispose()
    {
    }

    private void DetachAll()
    {
        base.ChangeTracker.Clear();
    }
}
