namespace Gizo.Domain.Aggregates.CarBrandAggregate;

public class CarBrand
{
    public CarBrand(string name)
    {
        Name = name;
    }
    public long Id { get; set; }

    public string Name { get; private set; }

    public bool IsAvailable { get; private set; }

    public IReadOnlyCollection<CarModel> CarModels { get; private set; } = null!;
}
