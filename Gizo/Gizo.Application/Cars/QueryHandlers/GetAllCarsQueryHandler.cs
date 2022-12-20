using Gizo.Application.Cars.Dtos;
using Gizo.Application.Cars.Queries;
using Gizo.Application.Models;
using Gizo.Domain.Aggregates.CarAggregate;
using Gizo.Domain.Contracts.Repository;
using MediatR;

namespace Gizo.Application.Cars.QueryHandlers;

public class GetAllCarsQueryHandler
    : IRequestHandler<GetAllCarsQuery, OperationResult<List<CarsDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Car> _carRepository;
    private readonly OperationResult<List<CarsDto>> _result = new();

    public GetAllCarsQueryHandler(
        IRepository<Car> carRepository,
        IUnitOfWork unitOfWork)
    {
        _carRepository = carRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<List<CarsDto>>> Handle(GetAllCarsQuery request, CancellationToken token)
    {
        var cars = await _carRepository
            .Get()
            .ProjectTo<CarsDto>()
            .ToListAsync(token);

        _result.Data = cars;

        return _result;
    }
}
