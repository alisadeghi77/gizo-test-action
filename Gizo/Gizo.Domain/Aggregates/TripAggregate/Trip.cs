using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Domain.Contracts.Base;
using Gizo.Domain.Contracts.Enums;

namespace Gizo.Domain.Aggregates.TripAggregate;

public class Trip : ICreateDate, IOptionalModifiedDate
{
    private readonly List<TripTempFile> _tripTempFiles = new();

    private Trip(long userId, int chunkSize)
    {
        UserId = userId;
        ChunkSize = chunkSize;
        CreateDate = DateTime.UtcNow;
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

    public string? TempFileName { get; private set; }

    public string? VideoFileName { get; private set; }

    public string? ImuFileName { get; private set; }

    public string? GpsFileName { get; private set; }

    public string? FilesPath { get; private set; }

    public DateTime? StartDateTime { get; private set; }

    public DateTime? EndDateTime { get; private set; }
    
    public DateTime CreateDate { get; private set; }

    public DateTime? ModifyDate { get; private set; }

    public User User { get; private set; }

    public UserCarModel UserCarModel { get; private set; }

    public IReadOnlyCollection<TripTempFile> TripTempFiles => _tripTempFiles;

    public static Trip CreateTrip(long userId,
        int chunkSize)
    {
        var trip = new Trip(userId, chunkSize);

        return trip;
    }

    public Trip UploadFileCompleted(Trip trip, TripFileEnum tripFile, int chunkCount)
    {
        IsValidChunkCount(chunkCount);

        trip.ModifyDate = DateTime.UtcNow;
        ChangeStatus(trip, tripFile);

        return trip;
    }

    public string GetFileType()
    {
        return TripTempFiles.FirstOrDefault()?.FileType;
    }

    public TripTempFile AddTempFiles(string fileName, string chunkId, string fileType, TripFileEnum tripFile)
    {
        var tempFile = new TripTempFile(Id, fileName, chunkId, fileType, tripFile);

        _tripTempFiles.Add(tempFile);

        return tempFile;
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

    private void IsValidChunkCount(int chunkCount)
    {
        if (TripTempFiles.Count != chunkCount)
        {
            throw new Exception("The file has not been fully uploaded yet");
        }
    }
}