using Gizo.Domain.Aggregates.CarBrandAggregate;

namespace Gizo.Domain.Aggregates.UserAggregate;

public class UserCarModel
{
    public UserCarModel(long userId, long carModelId , string license)
    {
        UserId = userId;
        CarModelId = carModelId;
        License = license;
    }

    public long Id { get; set; }

    public long UserId { get; private set; }

    public long CarModelId { get; private set; }

    public string License { get; set; }

    public User User { get; private set; }

    public CarModel CarModel { get; private set; }
}
