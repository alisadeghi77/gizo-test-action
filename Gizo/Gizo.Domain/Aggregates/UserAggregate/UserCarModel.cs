using Gizo.Domain.Aggregates.CarBrandAggregate;
using Gizo.Domain.Aggregates.TripAggregate;

namespace Gizo.Domain.Aggregates.UserAggregate;

public class UserCarModel
{
    public UserCarModel(long userId, long carModelId, string license)
    {
        UserId = userId;
        CarModelId = carModelId;
        License = license;
    }

    public long Id { get; set; }

    public long UserId { get; private set; }

    public long CarModelId { get; private set; }

    public string License { get; private set; }

    public bool IsSelected { get; private set; }

    public IReadOnlyCollection<Trip> Trips { get; private set; } = null!;

    public User User { get; private set; } = null!;

    public CarModel CarModel { get; private set; } = null!;

    public void SelectCarModel()
    {
        IsSelected = true;
    }

    public void RemoveSelectCarModel()
    {
        IsSelected = false;
    }

    public void SetLicense(string license)
    {
        License = license;
    }

    public bool IsDuplicate(long userId, long carModelId, string license)
    {
        return UserId == userId && CarModelId == carModelId && License == license;
    }
}
