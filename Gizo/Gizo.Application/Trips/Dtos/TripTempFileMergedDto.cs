namespace Gizo.Application.Trips.Dtos;

public class TripTempFileMergedDto
{
    public TripTempFileMergedDto(string fileName)
    {
        FileName = fileName;
    }

    public string FileName { get; set; }
}
