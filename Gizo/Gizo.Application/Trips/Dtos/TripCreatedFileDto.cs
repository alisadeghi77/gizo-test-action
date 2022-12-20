namespace Gizo.Application.Trips.Dtos;

public class TripCreatedFileDto
{
    public TripCreatedFileDto(
        string filePath,
        decimal chunkSize)
    {
        FilePath = filePath;
        ChunkSize = chunkSize;
    }

    public string FilePath { get; set; }
    public decimal ChunkSize { get; set; }
}
