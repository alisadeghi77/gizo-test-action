using Gizo.Application.Identity.Dtos;

namespace Gizo.Api.MappingProfiles;

public class IdentitMappings : Profile
{
    public IdentitMappings()
    {
        CreateMap<UserRegistrationRequest, RegisterIdentity>();
        CreateMap<LoginRequest, LoginCommand>();
        CreateMap<IdentityUserProfileDto, IdentityUserProfile>();
    }
}