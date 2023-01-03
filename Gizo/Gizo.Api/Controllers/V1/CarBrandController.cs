using Gizo.Api.Contracts.CarBrands.Responses;
using Gizo.Application.CarBrands.QueryHandlers;

namespace Gizo.Api.Controllers.V1;

public class CarBrandController : BaseController
{
    [HttpGet]
    [ValidateModel]
    public async Task<ActionResult<List<CarBrandResponse>>> GetCarBrands(CancellationToken cancellationToken)
    {
        var query = new GetCarBrandsQuery();
        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }
}
