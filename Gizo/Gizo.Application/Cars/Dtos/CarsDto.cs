using AutoMapper;
using Gizo.Domain.Aggregates.CarAggregate;

namespace Gizo.Application.Cars.Dtos;

public class CarsDto
{
    public string CarName { get; set; }

    public IReadOnlyList<CarModelsDto> CarModels { get; set; }
}
