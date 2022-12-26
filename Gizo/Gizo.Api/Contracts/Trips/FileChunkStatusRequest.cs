using Gizo.Domain.Contracts.Enums;

namespace Gizo.Api.Contracts.Trips;

public class FileChunkStatusRequest
{
    public long TripId { get; set; }

    public TripFileEnum TripFileType { get; set; }
}
