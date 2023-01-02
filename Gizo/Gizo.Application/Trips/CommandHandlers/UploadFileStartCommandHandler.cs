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
    TripFileEnum TripFileType) : IRequest<OperationResult<TripUploadStartResponse>>;

public class UploadFileStartCommandHandler
    : IRequestHandler<UploadFileStartCommand, OperationResult<TripUploadStartResponse>>
{
    private readonly DataContext _context;

    public UploadFileStartCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<TripUploadStartResponse>> Handle(UploadFileStartCommand request,
        CancellationToken token)
    {
        var result = new OperationResult<TripUploadStartResponse>();
        var trip = await _context.Trips.FirstOrDefaultAsync(_ => _.Id == request.TripId &&
            _.UserId == request.UserId, token);

        if (trip == null)
        {
            result.AddError(ErrorCode.NotFound, "Trip not found");

            return result;
        }

        trip.SetFileChunkCount(trip, request.TripFileType, request.ChunkCount);

        _context.Trips.Update(trip);
        await _context.SaveChangesAsync(token);
        result.Data = new TripUploadStartResponse(trip.Id);

        return result;
    }
}
