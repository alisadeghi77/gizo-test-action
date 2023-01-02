namespace Gizo.Application.Trips.Dtos;

public class TripImuDateTimeDto
{
    public TripImuDateTimeDto(DateTime tripStartDate, DateTime tripEndDate)
    {
        TripStartDate = tripStartDate;
        TripEndDate = tripEndDate;
    }

    public DateTime TripStartDate { get; set; }

    public DateTime TripEndDate { get; set; }
}
