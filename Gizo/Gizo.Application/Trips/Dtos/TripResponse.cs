namespace Gizo.Application.Trips.Dtos;

public class TripResponse
{
    public long Id { get; set; }

    public decimal Score { get; set; }

    public bool IsVideoUploaded { get; set; }

    public bool IsImuUploaded { get; set; }

    public bool IsGpsUploaded { get; set; }

    public DateTime CreateDate { get; set; }
}
