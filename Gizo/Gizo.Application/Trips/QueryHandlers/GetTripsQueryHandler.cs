using AutoMapper;
using Gizo.Application.Models;
using Gizo.Application.Trips.Dtos;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Trips.QueryHandlers;

public sealed record GetTripsQuery(long UserId) : IRequest<OperationResult<List<TripResponse>>>;

public class GetTripsQueryHandler : IRequestHandler<GetTripsQuery, OperationResult<List<TripResponse>>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetTripsQueryHandler(DataContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OperationResult<List<TripResponse>>> Handle(GetTripsQuery request, CancellationToken token)
    {
        var result = new OperationResult<List<TripResponse>>();
        var trips = await _context.Trips
            .Where(_ => _.UserId == request.UserId)
            .OrderByDescending(_ => _.CreateDate)
            .AsNoTracking()
            .Select(_ => _mapper.Map<TripResponse>(_))
            .ToListAsync(token);

        result.Data = trips;

        return result;
    }
}
