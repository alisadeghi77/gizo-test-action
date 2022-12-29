using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Domain.Contracts.Base;

namespace Gizo.Domain.Aggregates.CarBrandAggregate;

public class CarModel : BaseEntity<long>
{
    public long CarBrandId { get; set; }

    public string Name { get; set; }

    public CarBrand CarBrand { get; set; }

    public IReadOnlyCollection<UserCarModel> UserCarModels { get; private set; }
}
