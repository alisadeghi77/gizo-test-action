using Gizo.Application.Configs.Dtos;
using Gizo.Application.Configs.QueryHandlers;

namespace Gizo.Api.Controllers.V1;

[Authorize]
public class ConfigController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<GetConfigsResponse>> Get(CancellationToken token = default)
    {
        var query = new GetConfigsQuery();

        var result = await _mediator.Send(query, token);
        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }
}