using AutoMapper;
using Gizo.Application.Cars.Dtos;
using Gizo.Domain.Aggregates.CarAggregate;

namespace Gizo.Application.Cars.MappingProfiles;

public class CarMappingProfile : Profile
{
    public CarMappingProfile()
    {
        CreateMap<CarsDto, Car>().ReverseMap();
        CreateMap<CarModelsDto, CarModel>().ReverseMap();
    }
}
