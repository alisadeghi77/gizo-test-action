using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Aggregates.CarAggregate;

public class CarModel : BaseEntity<long>
{
    public long CarId { get; set; }

    public string ModelName { get; set; }

    public Car Car { get; set; }

    public IReadOnlyCollection<UserCar> UserCars { get; set; }
}
