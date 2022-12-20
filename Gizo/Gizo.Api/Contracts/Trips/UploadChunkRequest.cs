namespace Gizo.Api.Contracts.Trips;

public class UploadChunkRequest
{
    public long TripId { get; set; }

    public string FileChunkId { get; set; }

    public string FileName { get; set; }

    public IFormFile FileChunk { get; set; }
}
