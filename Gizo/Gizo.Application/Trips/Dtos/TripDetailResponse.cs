namespace Gizo.Application.Trips.Dtos;

public class TripDetailResponse
{
    public long Id { get; set; }

    public decimal Score { get; set; }

    public string? VideoFileName { get; set; }

    public string? ImuFileName { get; set; }

    public string? GpsFileName { get; set; }

    public bool IsVideoUploaded { get; set; }

    public bool IsImuUploaded { get; set; }

    public bool IsGpsUploaded { get; set; }

    public DateTime CreateDate { get; set; }
}
