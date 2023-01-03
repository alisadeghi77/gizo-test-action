using Gizo.Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Api.Registrars;

public class DbRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        var cs = builder.Configuration.GetConnectionString("Default");
        builder.Services.AddDbContext<DataContext>(options => { options.UseSqlServer(cs); });

        builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddUserManager<UserManager<User>>();
    }
}
