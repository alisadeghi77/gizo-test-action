namespace Gizo.Api.Contracts.Trips;

public class UploadCompletedRequest
{
    public long TripId { get; set; }

    public string FileName { get; set; }
}
