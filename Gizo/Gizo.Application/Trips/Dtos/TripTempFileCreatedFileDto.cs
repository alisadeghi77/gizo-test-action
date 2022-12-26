namespace Gizo.Application.Trips.Dtos;
public class TripTempFileCreatedFileDto
{
    public TripTempFileCreatedFileDto(string fileName)
    {
        FileName = fileName;
    }

    public string FileName { get; set; }
}
