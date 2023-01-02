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

    public Trip UploadFileCompleted(Trip trip, TripFileEnum tripFile)
    {
        IsValidChunkCount(tripFile);

        trip.ModifyDate = DateTime.UtcNow;
        ChangeStatus(trip, tripFile);

        return trip;
    }

    public Trip SetTripFilesFormat(Trip trip, string filePath, DateTime tripStartDate)
    {
        trip.FilesPath = filePath;
        trip.VideoFileName = $"{TripFileEnum.Video}-{tripStartDate.ToStandardDateTime()}.mp4";
        trip.ImuFileName = $"{TripFileEnum.IMU}-{tripStartDate.ToStandardDateTime()}.csv";
        trip.GpsFileName = $"{TripFileEnum.GPS}-{tripStartDate.ToStandardDateTime()}.csv";

        return trip;
    }

    public Trip SetUploadCompleted(Trip trip)
    {
        trip.IsCompleted = true;

        return trip;
    }

    public Trip SetTripDate(Trip trip, DateTime tripStartDate, DateTime tripEndDate)
    {
        trip.StartDateTime = tripStartDate;
        trip.EndDateTime = tripEndDate;

        return trip;
    }

    public Trip SetFileChunkCount(Trip trip, TripFileEnum tripFileType, int chunkCount)
    {
        switch (tripFileType)
        {
            case TripFileEnum.Video:
                trip.VideoChunkCount = chunkCount;
                break;
            case TripFileEnum.IMU:
                trip.ImuChunkCount = chunkCount;
                break;
            case TripFileEnum.GPS:
                trip.GpsChunkCount = chunkCount;
                break;
            default:
                break;
        }

        return trip;
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

    public bool IsCompletedUploadFile(Trip trip, TripFileEnum tripFile)
    {
        return tripFile switch
        {
            TripFileEnum.Video => trip.TripTempFiles.Count == trip.VideoChunkCount,
            TripFileEnum.IMU => trip.TripTempFiles.Count == trip.ImuChunkCount,
            TripFileEnum.GPS => trip.TripTempFiles.Count == trip.GpsChunkCount,
            _ => false,
        };
    }

    public void RemoveAllTempFiles()
    {
        _tripTempFiles.Clear();
    }

    private static void ChangeStatus(Trip trip, TripFileEnum tripFile)
    {
        switch (tripFile)
        {
            case TripFileEnum.Video:
                trip.IsVideoUploaded = true;
                break;
            case TripFileEnum.IMU:
                trip.IsImuUploaded = true;
                break;
            case TripFileEnum.GPS:
                trip.IsGpsUploaded = true;
                break;
            default:
                break;
        }
    }

    private void IsValidChunkCount(TripFileEnum tripFileType)
    {
        var getChunkCount = GetFileChunkCount(tripFileType);

        if (TripTempFiles.Count != getChunkCount)
        {
            throw new Exception("The file has not been fully uploaded yet");
        }
    }
}