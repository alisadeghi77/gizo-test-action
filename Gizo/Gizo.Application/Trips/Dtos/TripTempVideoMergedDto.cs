namespace Gizo.Application.Trips.Dtos;

public class TripTempVideoMergedDto
{
    public TripTempVideoMergedDto(string fileName)
    {
        FileName = fileName;
    }

    public string FileName { get; set; }
}
