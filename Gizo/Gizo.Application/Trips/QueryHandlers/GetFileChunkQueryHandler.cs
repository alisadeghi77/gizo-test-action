using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Trips.Dtos;
using Gizo.Domain.Contracts.Enums;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Trips.QueryHandlers;

public sealed record GetFileChunkQuery(long UserId,
    long TripId,
    TripFileType TripFileType) : IRequest<OperationResult<FileChunkStatusResponse>>;

public class GetFileChunkQueryHandler
    : IRequestHandler<GetFileChunkQuery, OperationResult<FileChunkStatusResponse>>
{
    private readonly DataContext _context;
    private readonly OperationResult<FileChunkStatusResponse> _result = new();

    public GetFileChunkQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<FileChunkStatusResponse>> Handle(GetFileChunkQuery request,
        CancellationToken cancellationToken)
    {
        var trip = await _context.Trips
            .Include(_ => _.TripTempFiles.Where(x => x.TripFileType == request.TripFileType))
            .AsNoTracking()
            .FirstOrDefaultAsync(_ => _.UserId == request.UserId && _.Id == request.TripId, cancellationToken);

        if (trip == null)
        {
            _result.AddError(ErrorCode.NotFound, "Trip not found");
            return _result;
        }

        var fileChunkIds = trip.TripTempFiles
            .Select(_ => _.ChunkId)
            .ToList();

        _result.Data = new FileChunkStatusResponse(fileChunkIds,
            trip.ChunkSize,
            trip.IsVideoUploaded,
            trip.IsImuUploaded,
            trip.IsGpsUploaded);

        return _result;
    }
}
