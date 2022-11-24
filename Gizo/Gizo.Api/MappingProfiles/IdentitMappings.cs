using Gizo.Application.Identity.Dtos;

namespace Gizo.Api.MappingProfiles;

public class IdentitMappings : Profile
{
    public IdentitMappings()
    {
        CreateMap<UserRegistration, RegisterIdentity>();
        CreateMap<Login, LoginCommand>();
        CreateMap<IdentityUserProfileDto, IdentityUserProfile>();
    }
}