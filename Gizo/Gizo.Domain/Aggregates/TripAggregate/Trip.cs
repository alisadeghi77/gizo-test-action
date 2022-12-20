using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Aggregates.TripAggregate;

public class Trip : BaseEntity<long>, ICreateDate, IOptionalModifiedDate
{

    private Trip(long userId, decimal chunkSize, string videoFilePath)
    {
        UserId = userId;
        ChunkSize = chunkSize;
        VideoFilePath = videoFilePath;
        CreateDate = DateTime.UtcNow;
    }

    public long UserId { get; private set; }

    public decimal Score { get; private set; }

    public bool IsVideoUploaded { get; private set; }

    public bool IsImuUploaded { get; private set; }

    public bool IsGpsUploaded { get; private set; }

    public decimal ChunkSize { get; private set; }

    public string? VideoFileName { get; private set; }

    public string VideoFilePath { get; private set; }

    public DateTime CreateDate { get; set; }

    public DateTime? ModifyDate { get; set; }

    public User User { get; set; }

    private readonly List<TripTempVideo> _tripTempVideos = new();

    public IReadOnlyCollection<TripTempVideo> TripTempVideos => _tripTempVideos;

    public static Trip CreateTrip(long userId,
        decimal chunkSize,
        string videoFilePath)
    {
        var trip = new Trip(userId, chunkSize, videoFilePath);

        return trip;
    }

    public static Trip UploadVideoCompleted(Trip trip,
        string videoFileName)
    {
        trip.VideoFileName = videoFileName;
        trip.ModifyDate = DateTime.UtcNow;

        return trip;
    }

    public TripTempVideo AddTempVideo(string fileName)
    {
        var tempVideo = new TripTempVideo(Id, fileName);

        _tripTempVideos.Add(tempVideo);

        return tempVideo;
    }

    public void RemoveAllTempVideos()
    {
        _tripTempVideos.Clear();
    }
}
