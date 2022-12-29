using AutoMapper;
using Gizo.Application.Users.Dtos;
using Gizo.Domain.Aggregates.UserAggregate;

namespace Gizo.Application.Users.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserProfileResponse, User>().ReverseMap();
    }
}
