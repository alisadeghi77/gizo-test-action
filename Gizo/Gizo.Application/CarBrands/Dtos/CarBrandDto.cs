namespace Gizo.Application.CarBrands.Dtos;

public class CarBrandDto
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public IReadOnlyList<CarModelDto>? CarModels { get; set; }
}
