using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Aggregates.UserAggregate;

public class UserLocation : ICreateDate
{
    public Guid Id { get; set; }

    public long UserId { get; set; }

    public DateTime GpsDateTime { get; set; }

    public double Altitude { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double Speed { get; set; }

    public double Climb { get; set; }

    public DateTime CreateDate { get; set; }

    public User User { get; private set; }
}
