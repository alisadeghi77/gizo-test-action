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
        if (!_uploadFileService.CheckVideoType(request.TripFileType, request.Type))
        {
            _result.AddError(ErrorCode.ValidationError, "File type is not valid");
            return _result;
        }

        var trip = await _context.Trips
            .Include(_ => _.TripTempFiles
                .Where(x => x.TripFileType == request.TripFileType))
            .Include(_ => _.UserCarModel.CarModel)
            .FirstOrDefaultAsync(_ => _.Id == request.TripId, token);

        if (trip == null)
        {
            _result.AddError(ErrorCode.NotFound, "Trip not found");
            return _result;
        }

        var tempPath = GetTripTempFilePath(trip, request.TripFileType);
        var tempFileName = await SaveChunkFile(trip, request, tempPath);
        var tempFile = trip.AddTempFiles(tempFileName, request.FileChunkId, request.Type, request.TripFileType);

        if (trip.IsCompletedUploadFile(trip, request.TripFileType))
        {
            MergeChunkFiles(trip, request, tempPath);
        }

        if (trip.AreAllFilesUploaded())
        {
            trip = CompleteUploadFiles(trip, request, tempPath);
        }

        _context.Trips.Update(trip);
        await _context.SaveChangesAsync(token);

        _result.Data = new TripTempFileCreatedResponse()
        {
            Id = trip.Id,
        };

        return _result;
    }

    public async Task<string> SaveChunkFile(Trip trip, UploadFileChunkCommand request, string tempPath)
    {
        var fileName = CreateFileChunkName(trip.TempFileName, request.FileChunkId, request.Type);

        await _uploadFileService.UploadChunk(fileName, tempPath, request.File, trip.ChunkSize);

        return fileName;
    }

    public Trip MergeChunkFiles(Trip trip, UploadFileChunkCommand request, string tempPath)
    {
        _uploadFileService.UploadCompleted(tempPath, trip.TempFileName, request.Type);

        trip = trip.UploadFileCompleted(trip, request.TripFileType);

        return trip;
    }

    private static string CreateFileChunkName(string fileName, string fileChunkId, string fileType)
    {
        return fileName + fileType.ToStandardType() + fileChunkId;
    }

    private string GetTripTempFilePath(Trip trip, TripFileEnum tripFileType)
    {
        var tempPath = _uploadFileService.GetTripTempFilePath(_uploadFileSettings.SaveTempTo,
           trip.UserId, trip.Id,
           tripFileType.ToString());

        return tempPath;
    }

    private Trip CompleteUploadFiles(Trip trip, UploadFileChunkCommand request, string tempPath)
    {
        var imuTripDate = _uploadFileService.ImuStartAndEndDateTime(trip.UserId,
            trip.Id,
            _uploadFileSettings.SaveTempTo);

        if (imuTripDate != null)
        {
            var folderPath = _uploadFileService.CreateTripFolder(_uploadFileSettings.FolderStructure,
                trip.UserId,
                trip.UserCarModel.CarModel.Name,
                trip.UserCarModel.License,
                imuTripDate.TripStartDate);

            trip = trip.SetTripDate(trip, imuTripDate.TripStartDate, imuTripDate.TripEndDate);
            trip = trip.SetTripFilesFormat(trip, folderPath, imuTripDate.TripStartDate);

            _uploadFileService.MoveFiles(GetTripTempFilePath(trip, TripFileEnum.Video),
                folderPath,
                trip.VideoFileName,
                TripFileEnum.Video);

            _uploadFileService.MoveFiles(GetTripTempFilePath(trip, TripFileEnum.IMU),
                folderPath,
                trip.ImuFileName,
                TripFileEnum.IMU);

            _uploadFileService.MoveFiles(GetTripTempFilePath(trip, TripFileEnum.GPS),
                folderPath,
                trip.GpsFileName,
                TripFileEnum.GPS);

            trip.SetUploadCompleted(trip);
            trip.RemoveAllTempFiles();
        }

        return trip;
    }
}
