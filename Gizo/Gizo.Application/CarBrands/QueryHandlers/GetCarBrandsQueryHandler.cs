using AutoMapper;
using Gizo.Application.CarBrands.Dtos;
using Gizo.Application.Models;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.CarBrands.QueryHandlers;

public sealed record GetCarBrandsQuery() : IRequest<OperationResult<List<CarBrandDto>>>;

public class GetCarBrandsQueryHandler
    : IRequestHandler<GetCarBrandsQuery, OperationResult<List<CarBrandDto>>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly OperationResult<List<CarBrandDto>> _result = new();

    public GetCarBrandsQueryHandler(IMapper mapper,
        DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<OperationResult<List<CarBrandDto>>> Handle(GetCarBrandsQuery request, CancellationToken token)
    {
        var cars = await _context.Cars
            .AsNoTracking()
            .Include(_ => _.CarModels)
            .Select(_ => _mapper.Map<CarBrandDto>(_))
            .ToListAsync(token);

        _result.Data = cars;
        return _result;
    }
}
