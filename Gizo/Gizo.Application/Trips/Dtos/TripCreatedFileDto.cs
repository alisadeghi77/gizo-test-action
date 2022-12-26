namespace Gizo.Application.Trips.Dtos;

public class TripCreatedFileDto
{
    public TripCreatedFileDto(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; set; }
}
