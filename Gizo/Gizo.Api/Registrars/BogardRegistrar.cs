namespace Gizo.Api.Registrars;

public class BogardRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllUserProfilesQuery));
        builder.Services.AddMediatR(typeof(GetAllUserProfilesQuery));
    }
}
