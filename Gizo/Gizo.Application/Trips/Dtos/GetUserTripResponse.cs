namespace Gizo.Application.Trips.Dtos;

public class GetUserTripResponse
{
    public long Id { get; set; }
    public decimal Score { get; private set; }

    public bool IsVideoUploaded { get; private set; }

    public bool IsImuUploaded { get; private set; }

    public bool IsGpsUploaded { get; private set; }

    public DateTime CreateDate { get; set; }
}
