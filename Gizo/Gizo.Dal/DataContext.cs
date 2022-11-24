using Gizo.Domain.Aggregates.PostAggregate;
using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Dal.Configurations;
using Gizo.Dal.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Dal
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions options) : base(options)    
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<BasicInfo>();
            modelBuilder.ApplyAllConfigurations();
            base.OnModelCreating(modelBuilder);
        }
    }
}
