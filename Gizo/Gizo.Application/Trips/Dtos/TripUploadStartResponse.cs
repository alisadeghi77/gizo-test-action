namespace Gizo.Application.Trips.Dtos;

public class TripUploadStartResponse
{
    public TripUploadStartResponse(long tripId)
    {
        TripId = tripId;
    }

    public long TripId { get; set; }
}
