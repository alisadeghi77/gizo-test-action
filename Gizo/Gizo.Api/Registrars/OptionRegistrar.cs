namespace Gizo.Api.Registrars;

public class OptionRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.Configure<UploadFileSettings>(builder.Configuration.GetSection(nameof(UploadFileSettings)));
    }
}
