using Gizo.Api.Contracts.CarBrands.Responses;
using Gizo.Application.CarBrands.QueryHandlers;

namespace Gizo.Api.Controllers.V1;

public class CarBrandController : BaseController
{
    [HttpGet]
    [ValidateModel]
    public async Task<ActionResult<List<CarBrandResponse>>> GetCarBrands(CancellationToken token)
    {
        var query = new GetCarBrandsQuery();
        var result = await _mediator.Send(query, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }
}
