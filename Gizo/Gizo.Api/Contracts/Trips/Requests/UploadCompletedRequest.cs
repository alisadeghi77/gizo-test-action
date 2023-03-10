using Gizo.Domain.Contracts.Enums;

namespace Gizo.Api.Contracts.Trips.Requests;

public class UploadCompletedRequest
{
    public long TripId { get; set; }

    public TripFileType TripFileType { get; set; }

    public int ChunkLenght { get; set; }
}
