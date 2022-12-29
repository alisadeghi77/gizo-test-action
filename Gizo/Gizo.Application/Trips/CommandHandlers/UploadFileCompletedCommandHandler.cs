using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Options;
using Gizo.Application.Services;
using Gizo.Domain.Aggregates.TripAggregate;
using Gizo.Domain.Contracts.Enums;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Gizo.Application.Trips.CommandHandlers;

public sealed record UploadFileCompletedCommand(long Id,
    TripFileEnum TripFileType,
    int ChunkCount) : IRequest<OperationResult<bool>>;

public class UploadFileCompletedCommandHandler
    : IRequestHandler<UploadFileCompletedCommand, OperationResult<bool>>
{
    private readonly DataContext _context;
    private readonly UploadFileService _uploadFileService;
    private readonly OperationResult<bool> _result = new();
    private readonly UploadFileSettings _uploadFileSettings;

    public UploadFileCompletedCommandHandler(UploadFileService uploadFileService,
        DataContext context,
        IOptions<UploadFileSettings> uploadFileSettings)
    {
        _context = context;
        _uploadFileService = uploadFileService;
        _uploadFileSettings = uploadFileSettings.Value;
    }

    public async Task<OperationResult<bool>> Handle(UploadFileCompletedCommand request,
        CancellationToken token)
    {
        var trip = await _context.Trips
            .Include(_ => _.TripTempFiles.Where(x => x.TripFileType == request.TripFileType))
            .FirstOrDefaultAsync(_ => _.Id == request.Id, token);

        if (trip == null)
        {
            _result.AddError(ErrorCode.NotFound, "Trip not found");
            return _result;
        }

        var tempPath = _uploadFileService.GetTripTempFilePath(_uploadFileSettings.SaveTempTo,
                      trip.UserId, trip.Id,
                      request.TripFileType.ToString());

        var type = trip.GetFileType();

        var fileName = _uploadFileService.UploadCompleted(tempPath, trip.TempFileName, type);

        var updatedModel = trip.UploadFileCompleted(trip, request.TripFileType, request.ChunkCount);

        trip.RemoveAllTempFiles();

        _context.Trips.Update(updatedModel);

        _result.Data = await _context.SaveChangesAsync(token) > 0;

        return _result;
    }
}
