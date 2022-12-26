using AutoMapper;
using Gizo.Application.Models;
using Gizo.Application.Trips.Dtos;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Trips.QueryHandlers;

public sealed record GetAllTripsQuery(long UserId) : IRequest<OperationResult<List<GetUserTripResponse>>>;

public class GetAllTripsQueryHandler : IRequestHandler<GetAllTripsQuery, OperationResult<List<GetUserTripResponse>>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetAllTripsQueryHandler(DataContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OperationResult<List<GetUserTripResponse>>> Handle(GetAllTripsQuery request, CancellationToken token)
    {
        var result = new OperationResult<List<GetUserTripResponse>>();
        var trips = await _context.Trips
            .Where(_ => _.UserId == request.UserId)
            .OrderByDescending(_ => _.CreateDate)
            .Select(_ => _mapper.Map<GetUserTripResponse>(_))
            .ToListAsync(token);

        result.Data = trips;

        return result;
    }
}
