using Gizo.Api.Contracts.ClientIdentity;
using Gizo.Application.ClientIdentity.Commands;
using Gizo.Application.ClientIdentity.Dtos;
using Gizo.Application.Identity.Dtos;

namespace Gizo.Api.MappingProfiles;

public class ClientIdentitMappings : Profile
{
    public ClientIdentitMappings()
    {
        CreateMap<CheckClientIdentityRequest, CheckClientIdentityCommand>().ReverseMap();
        CreateMap<VerifyClientIdentityRequest, VerifyClientIdentityCommand>().ReverseMap();
        CreateMap<VerifyClientIdentityResult, ClientIdentityUserDto>().ReverseMap();
    }
}
