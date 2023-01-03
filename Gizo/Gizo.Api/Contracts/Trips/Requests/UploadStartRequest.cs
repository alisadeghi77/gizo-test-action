using Gizo.Domain.Contracts.Enums;

namespace Gizo.Api.Contracts.Trips.Requests;

public class UploadStartRequest
{
    public long TripId { get; set; }
    public int ChunkCount { get; set; }
    public TripFileType TripFileType { get; set; }
}
