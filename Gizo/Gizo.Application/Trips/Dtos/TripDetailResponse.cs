namespace Gizo.Application.Trips.Dtos;

public class TripDetailResponse
{
    public long Id { get; set; }

    public decimal Score { get; set; }

    public decimal HarshAccelerationScore { get; set; }

    public decimal HarshBrakingScore { get; set; }

    public decimal HarshCorneringScore { get; set; }

    public decimal SpeedingScore { get; set; }

    public decimal TailGatingScore { get; set; }

    public decimal RedLightCrossingScore { get; set; }

    public decimal StopSignCrossingScore { get; set; }

    public decimal RideDurationScore { get; set; }

    public decimal RideDistanceScore { get; set; }

    public string? VideoFileName { get; set; }

    public string? ImuFileName { get; set; }

    public string? GpsFileName { get; set; }

    public bool IsVideoUploaded { get; set; }

    public bool IsImuUploaded { get; set; }

    public bool IsGpsUploaded { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }

    public TimeSpan? TripTime => StartDateTime.HasValue && EndDateTime.HasValue ? EndDateTime - StartDateTime : null;

    public DateTime CreateDate { get; set; }
}
