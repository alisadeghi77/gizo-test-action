using Gizo.Domain.Contracts.Enums;

namespace Gizo.Api.Contracts.Trips.Requests;

public class UploadChunkRequest
{
    public long TripId { get; set; }

    public string FileChunkId { get; set; }

    public TripFileType TripFileType { get; set; }

    public IFormFile FileChunk { get; set; }
}
