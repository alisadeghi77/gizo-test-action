namespace Gizo.Application.Trips.Dtos;
public class TripTempVideoCreatedFileDto
{
    public TripTempVideoCreatedFileDto(string fileName)
    {
        FileName = fileName;
    }

    public string FileName { get; set; }
}
