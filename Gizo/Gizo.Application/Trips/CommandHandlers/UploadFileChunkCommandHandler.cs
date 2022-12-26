using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Options;
using Gizo.Application.Services;
using Gizo.Application.Trips.Dtos;
using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Domain.Contracts.Enums;
using Gizo.Domain.Contracts.Repository;
using Gizo.Infrastructure;
using Gizo.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Gizo.Application.Trips.CommandHandlers;

public sealed record UploadFileChunkCommand(long TripId,
    string FileChunkId,
    TripFileEnum TripFileType,
    string Type,
    Stream File) : IRequest<OperationResult<TripTempFileCreatedResponse>>;

public class UploadFileChunkCommandHandler
    : IRequestHandler<UploadFileChunkCommand, OperationResult<TripTempFileCreatedResponse>>
{
    private readonly DataContext _context;
    private readonly UploadFileService _uploadFileService;
    private readonly OperationResult<TripTempFileCreatedResponse> _result = new();
    private readonly UploadFileSettings _uploadFileSettings;

    public UploadFileChunkCommandHandler(DataContext context,
        UploadFileService uploadFileService,
        IOptions<UploadFileSettings> uploadFileSettings)
    {
        _context = context;
        _uploadFileService = uploadFileService;
        _uploadFileSettings = uploadFileSettings.Value;
    }

    public async Task<OperationResult<TripTempFileCreatedResponse>> Handle(
        UploadFileChunkCommand request,
        CancellationToken token)
    {
        var trip = await _context.Trips
            .Include(x => x.TripTempFiles)
            .FirstOrDefaultAsync(_ => _.Id == request.TripId, token);

        if (trip == null)
        {
            _result.AddError(ErrorCode.NotFound, "Trip not found");
            return _result;
        }

        var fileName = CreateFileChunkName(trip.TempFileName, request.FileChunkId, request.Type);

        var tempPath = _uploadFileService.GetTripTempFilePath(_uploadFileSettings.SaveTempTo,
                trip.UserId, trip.Id,
                request.TripFileType.ToString());

        await _uploadFileService.UploadChunk(fileName, tempPath, request.File, trip.ChunkSize);

        var tempFile = trip.AddTempFiles(fileName, request.FileChunkId, request.Type, request.TripFileType);

        _context.Trips.Update(trip);
        await _context.SaveChangesAsync(token);

        _result.Data = new TripTempFileCreatedResponse()
        {
            Id = trip.Id,
        };

        return _result;
    }

    private static string CreateFileChunkName(string fileName, string fileChunkId, string fileType)
    {
        return fileName + fileType.ToStandardType() + fileChunkId;
    }
}
