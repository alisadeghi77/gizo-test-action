using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Trips.Dtos;
using Gizo.Domain.Contracts.Enums;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Trips.CommandHandlers;

public sealed record UploadFileStartCommand(long TripId,
    long UserId,
    int ChunkCount,
    TripFileType TripFileType) : IRequest<OperationResult<TripUploadStartResponse>>;

public class UploadFileStartCommandHandler
    : IRequestHandler<UploadFileStartCommand, OperationResult<TripUploadStartResponse>>
{
    private readonly DataContext _context;

    public UploadFileStartCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<TripUploadStartResponse>> Handle(UploadFileStartCommand request,
        CancellationToken cancellationToken)
    {
        var result = new OperationResult<TripUploadStartResponse>();
        var trip = await _context.Trips
            .Include(_ => _.TripTempFiles
                .Where(_ => _.TripFileType == request.TripFileType))
            .FirstOrDefaultAsync(_ => _.Id == request.TripId &&
            _.UserId == request.UserId, cancellationToken);

        if (trip == null)
        {
            result.AddError(ErrorCode.NotFound, "Trip not found");

            return result;
        }

        trip.SetFileChunkCount(request.TripFileType, request.ChunkCount);
        trip.RemoveAllTempFiles();

        _context.Trips.Update(trip);
        await _context.SaveChangesAsync(cancellationToken);
        result.Data = new TripUploadStartResponse(trip.Id);

        return result;
    }
}
