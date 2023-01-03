using AutoMapper;
using Gizo.Application.Users.Dtos;
using Gizo.Domain.Aggregates.UserAggregate;

namespace Gizo.Application.Users.MappingProfiles;

public class UserCarMappingProfile : Profile
{
    public UserCarMappingProfile()
    {
        CreateMap<UserCarModel, UserCarResponse>()
            .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => src.CarModel.CarBrand.Name))
            .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.CarModel.Name));
    }
}
