namespace Gizo.Api.Contracts.CarBrands.Responses;

public class CarBrandResponse
{
    public long Id { get; set; }

    public string Name { get; set; }

    public IReadOnlyList<CarModelResponse> CarModels { get; set; }
}