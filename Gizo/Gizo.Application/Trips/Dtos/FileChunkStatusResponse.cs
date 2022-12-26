namespace Gizo.Application.Trips.Dtos;

public class FileChunkStatusResponse
{
    public FileChunkStatusResponse(List<string> fileChunkIds,
        int chunkSize,
        bool isVideoUploaded,
        bool isImuUploaded,
        bool isGpsUploaded)
    {
        FileChunkIds = fileChunkIds;
        ChunkSize = chunkSize;
        IsVideoUploaded = isVideoUploaded;
        IsImuUploaded = isImuUploaded;
        IsGpsUploaded = isGpsUploaded;
    }

    public List<string> FileChunkIds { get; set; }

    public int ChunkSize { get; set; }

    public bool IsVideoUploaded { get; set; }

    public bool IsImuUploaded { get; set; }

    public bool IsGpsUploaded { get; set; }
}
