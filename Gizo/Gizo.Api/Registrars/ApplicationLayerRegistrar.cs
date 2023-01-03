using Gizo.Application.Services;
using Gizo.Domain.Contracts.Services;

namespace Gizo.Api.Registrars;

public class ApplicationLayerRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.Configure<IdentityConfigs>(builder.Configuration.GetSection(nameof(IdentityConfigs)));
        builder.Services.AddSingleton(service => service.GetRequiredService<IOptions<IdentityConfigs>>().Value);

        builder.Services.Configure<SmsConfigs>(builder.Configuration.GetSection(nameof(SmsConfigs)));
        builder.Services.AddSingleton(service => service.GetRequiredService<IOptions<SmsConfigs>>().Value);

        builder.Services.AddScoped<IdentityService>();
        builder.Services.AddScoped<UploadFileService>();
        builder.Services.AddScoped<ISmsService, SmsService>();
    }
}
