using Gizo.Domain.Contracts.Enums;

namespace Gizo.Api.Contracts.Trips.Requests;

public class FileChunkStatusRequest
{
    public long TripId { get; set; }

    public TripFileType TripFileType { get; set; }
}
