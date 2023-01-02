using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Contracts.Enums;
using Gizo.Utility;

namespace Gizo.Domain.Aggregates.TripAggregate;

public class Trip : ICreateDate, IOptionalModifiedDate
{
    private readonly List<TripTempFile> _tripTempFiles = new();

    protected Trip()
    {

    }

    private Trip(long userId, int chunkSize, long userCarModelId)
    {
        UserId = userId;
        ChunkSize = chunkSize;
        CreateDate = DateTime.UtcNow;
        UserCarModelId = userCarModelId;
        TempFileName = Guid.NewGuid().ToString();
    }

    public long Id { get; private set; }

    public long UserId { get; private set; }

    public long? UserCarModelId { get; private set; }

    public decimal Score { get; private set; }

    public decimal HarshAccelerationScore { get; private set; }

    public decimal HarshBrakingScore { get; private set; }

    public decimal HarshCorneringScore { get; private set; }

    public decimal SpeedingScore { get; private set; }

    public decimal TailGatingScore { get; private set; }

    public decimal RedLightCrossingScore { get; private set; }

    public decimal StopSignCrossingScore { get; private set; }

    public decimal RideDurationScore { get; private set; }

    public decimal RideDistanceScore { get; private set; }

    public bool IsVideoUploaded { get; private set; }

    public bool IsImuUploaded { get; private set; }

    public bool IsGpsUploaded { get; private set; }

    public int ChunkSize { get; private set; }

    public string TempFileName { get; private set; }

    public string? VideoFileName { get; private set; }

    public string? ImuFileName { get; private set; }

    public string? GpsFileName { get; private set; }

    public int VideoChunkCount { get; private set; }

    public int ImuChunkCount { get; private set; }

    public int GpsChunkCount { get; private set; }

    public bool IsCompleted { get; private set; }

    public string? FilesPath { get; private set; }

    public DateTime? StartDateTime { get; private set; }

    public DateTime? EndDateTime { get; private set; }

    public DateTime CreateDate { get; private set; }

    public DateTime? ModifyDate { get; private set; }

    public User User { get; private set; }

    public UserCarModel UserCarModel { get; private set; }

    public IReadOnlyCollection<TripTempFile> TripTempFiles => _tripTempFiles;

    public static Trip CreateTrip(long userId,
        int chunkSize,
        long userCarModelId)
    {
        var trip = new Trip(userId, chunkSize, userCarModelId);

        return trip;
    }

    public void UploadFileCompleted(TripFileEnum tripFile)
    {
        IsValidChunkCount(tripFile);

        ModifyDate = DateTime.UtcNow;
        ChangeStatus(tripFile);
    }

    public void SetTripFilesFormat(string filePath)
    {
        FilesPath = filePath;
        VideoFileName = $"{TripFileEnum.Video}{GetTempFileType(TripFileEnum.Video)}";
        ImuFileName = $"{TripFileEnum.IMU}{GetTempFileType(TripFileEnum.IMU)}";
        GpsFileName = $"{TripFileEnum.GPS}{GetTempFileType(TripFileEnum.GPS)}";
    }

    public void SetUploadCompleted()
    {
        IsCompleted = true;
    }

    public void SetTripDate(DateTime tripStartDate, DateTime tripEndDate)
    {
        StartDateTime = tripStartDate;
        EndDateTime = tripEndDate;
    }

    public void SetFileChunkCount(TripFileEnum tripFileType, int chunkCount)
    {
        switch (tripFileType)
        {
            case TripFileEnum.Video:
                VideoChunkCount = chunkCount;
                break;
            case TripFileEnum.IMU:
                ImuChunkCount = chunkCount;
                break;
            case TripFileEnum.GPS:
                GpsChunkCount = chunkCount;
                break;
            default:
                break;
        }
    }

    public int GetFileChunkCount(TripFileEnum tripFileType)
    {
        return tripFileType switch
        {
            TripFileEnum.Video => VideoChunkCount,
            TripFileEnum.IMU => ImuChunkCount,
            TripFileEnum.GPS => GpsChunkCount,
            _ => 0,
        };
    }

    public string GetFileType()
    {
        return TripTempFiles.FirstOrDefault()?.FileType;
    }

    public bool AreAllFilesUploaded()
    {
        return IsVideoUploaded &&
            IsImuUploaded &&
            IsGpsUploaded;
    }

    public TripTempFile AddTempFiles(string fileName, string chunkId, string fileType, TripFileEnum tripFile)
    {
        var tempFile = new TripTempFile(Id, fileName, chunkId, fileType, tripFile);

        _tripTempFiles.Add(tempFile);

        return tempFile;
    }

    public bool IsCompletedUploadFile(TripFileEnum tripFile)
    {
        return tripFile switch
        {
            TripFileEnum.Video => TripTempFiles.Count(_ => _.TripFileType == TripFileEnum.Video) == VideoChunkCount,
            TripFileEnum.IMU => TripTempFiles.Count(_ => _.TripFileType == TripFileEnum.IMU)  == ImuChunkCount,
            TripFileEnum.GPS => TripTempFiles.Count(_ => _.TripFileType == TripFileEnum.GPS)  == GpsChunkCount,
            _ => false,
        };
    }

    public void RemoveAllTempFiles()
    {
        _tripTempFiles.Clear();
    }

    private void ChangeStatus(TripFileEnum tripFile)
    {
        switch (tripFile)
        {
            case TripFileEnum.Video:
                IsVideoUploaded = true;
                break;
            case TripFileEnum.IMU:
                IsImuUploaded = true;
                break;
            case TripFileEnum.GPS:
                IsGpsUploaded = true;
                break;
            default:
                break;
        }
    }

    private void IsValidChunkCount(TripFileEnum tripFileType)
    {
        var getChunkCount = GetFileChunkCount(tripFileType);

        if (TripTempFiles.Count(_ => _.TripFileType == tripFileType) != getChunkCount)
        {
            throw new Exception("The file has not been fully uploaded yet");
        }
    }

    private string GetTempFileType(TripFileEnum tripFileType)
    {
        var tripFile = TripTempFiles.FirstOrDefault(_ => _.TripFileType == tripFileType);

        if (tripFile == null)
        {
            throw new Exception("Trip temp files not found");
        }

        return tripFile.FileType.ToStandardType();
    }
}