using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Aggregates.TripAggregate;

public class TripTempVideo : BaseEntity<long>, ICreateDate
{

    internal TripTempVideo(long tripId,string fileName)
    {
        TripId = tripId;
        FileName = fileName;
        CreateDate = DateTime.UtcNow;
    }

    public long TripId { get; private set; }

    public string FileName { get; private set; }

    public DateTime CreateDate { get; set; }

    public Trip Trip { get; set; }
  
}
