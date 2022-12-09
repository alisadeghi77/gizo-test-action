using Gizo.Application.Identity.Dtos;

namespace Gizo.Api.MappingProfiles;

public class IdentityMappings : Profile
{
    public IdentityMappings()
    {
        CreateMap<UserRegistrationRequest, RegisterIdentityCommand>();
        CreateMap<LoginRequest, LoginCommand>();
    }
}