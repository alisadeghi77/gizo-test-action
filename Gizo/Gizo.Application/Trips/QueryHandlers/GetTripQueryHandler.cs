using AutoMapper;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Trips.Dtos;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Trips.QueryHandlers;

public sealed record GetTripQuery(long TripId,
    long UserId) : IRequest<OperationResult<TripDetailResponse>>;

public class GetTripQueryHandler : IRequestHandler<GetTripQuery, OperationResult<TripDetailResponse>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetTripQueryHandler(DataContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OperationResult<TripDetailResponse>> Handle(GetTripQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<TripDetailResponse>();
        var trip = await _context.Trips
            .AsNoTracking()
            .FirstOrDefaultAsync(_ => _.Id == request.TripId &&
                _.UserId == request.UserId, cancellationToken);

        if (trip == null)
        {
            result.AddError(ErrorCode.NotFound, "Trip not found");

            return result;
        }

        result.Data = _mapper.Map<TripDetailResponse>(trip);

        return result;
    }
}
