using CsvHelper.Configuration.Attributes;

namespace Gizo.Application.Trips.Dtos;

public class TripImuReadCsvDto
{
    [Index(0)]
    public DateTime Time { get; set; }
}
