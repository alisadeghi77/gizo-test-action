namespace Gizo.Application.Trips.Dtos;

public class CreatedTripResponse
{
    public CreatedTripResponse(long tripId, int chunkSize)
    {
        TripId = tripId;
        ChunkSize = chunkSize;
    }

    public long TripId { get; set; }

    public int ChunkSize { get; set; }
}
