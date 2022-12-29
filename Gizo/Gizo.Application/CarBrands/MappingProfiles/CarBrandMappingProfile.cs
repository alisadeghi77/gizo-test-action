using AutoMapper;
using Gizo.Application.CarBrands.Dtos;
using Gizo.Domain.Aggregates.CarBrandAggregate;

namespace Gizo.Application.Cars.MappingProfiles;

public class CarBrandMappingProfile : Profile
{
    public CarBrandMappingProfile()
    {
        CreateMap<CarBrand, CarBrandDto>();
        CreateMap<CarModel, CarModelDto>();
    }
}
