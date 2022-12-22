using Gizo.Domain.Aggregates.CarAggregate;

namespace Gizo.Domain.Aggregates.UserAggregate;

public class UserCar
{
    public long UserId { get; private set; }

    public long CarModelId { get; private set; }

    public User User { get; private set; }

    public CarModel CarModel { get; private set; }
}
