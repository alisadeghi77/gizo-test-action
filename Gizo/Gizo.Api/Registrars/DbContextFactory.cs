using Microsoft.EntityFrameworkCore.Design;

namespace Gizo.Infrastructure.Context.ContextFactory;

public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var connectionString = GetAppConfiguration().GetConnectionString("Default");
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new DataContext(optionsBuilder.Options);
    }

    private static IConfiguration GetAppConfiguration()
    {
        var environmentName =
            Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT");

        var path = Directory.GetCurrentDirectory();

        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environmentName}.json", true)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
