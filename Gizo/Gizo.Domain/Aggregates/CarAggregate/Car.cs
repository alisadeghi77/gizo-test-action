using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Aggregates.CarAggregate;

public class Car : BaseEntity<long>
{
    public string CarName { get; private set; }

    public bool IsAvailable { get; private set; }

    public IReadOnlyCollection<CarModel> CarModels { get; private set; }
}
