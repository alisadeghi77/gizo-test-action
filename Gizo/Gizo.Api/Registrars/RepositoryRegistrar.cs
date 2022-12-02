using Gizo.Domain.Contracts.Repository;
using Gizo.Infrastructure.Repository;

namespace Gizo.Api.Registrars;

public class RepositoryRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}