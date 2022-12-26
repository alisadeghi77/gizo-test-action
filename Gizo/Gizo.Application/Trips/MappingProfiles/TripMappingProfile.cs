using AutoMapper;
using Gizo.Application.Trips.Dtos;
using Gizo.Domain.Aggregates.TripAggregate;

namespace Gizo.Application.Trips.MappingProfiles;

public class TripMappingProfile : Profile
{
    public TripMappingProfile()
    {
        CreateMap<Trip, GetUserTripResponse>();
    }
}
