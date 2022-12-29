using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Aggregates.CarBrandAggregate;

public class CarBrand
{
    public long Id { get; set; }

    public string Name { get; private set; }

    public bool IsAvailable { get; private set; }

    public IReadOnlyCollection<CarModel> CarModels { get; private set; }
}
