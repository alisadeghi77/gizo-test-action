using Gizo.Api.Contracts.Users;
using Gizo.Api.Contracts.Users.Requests;
using Gizo.Application.Users.CommandHandlers;
using Gizo.Application.Users.Dtos;

namespace Gizo.Api.MappingProfiles;

public class UserMappings : Profile
{
    public UserMappings()
    {
        CreateMap<UserRegistrationRequest, RegisterIdentityCommand>();
        CreateMap<LoginRequest, LoginCommand>();
        CreateMap<CheckIdentityRequest, CheckClientIdentityCommand>().ReverseMap();
        CreateMap<VerifyIdentityRequest, VerifyCommand>().ReverseMap();
        CreateMap<VerifyIdentityResponse, UserVerifyResponse>().ReverseMap();
    }
}
