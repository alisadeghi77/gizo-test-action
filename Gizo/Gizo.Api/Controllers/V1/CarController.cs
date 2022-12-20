using Gizo.Api.Contracts.Cars.Requests;
using Gizo.Application.Cars.Queries;

namespace Gizo.Api.Controllers.V1;

public class CarController : BaseController
{
    [HttpGet]
    [ValidateModel]
    public async Task<ActionResult> GetAllCars(CancellationToken token)
    {
        var query = new GetAllCarsQuery();
        var result = await _mediator.Send(query, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }
}
