namespace Gizo.Application.Trips.Dtos;

public class CreatedTripResponse
{
    public CreatedTripResponse(long tripId)
    {
        TripId = tripId;
    }

    public long TripId { get; set; }

}
