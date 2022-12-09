namespace Gizo.Api.Registrars;

public class AddCors : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(_ =>
        {
            _.AddDefaultPolicy(cors => cors
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());
        });
    }
}
