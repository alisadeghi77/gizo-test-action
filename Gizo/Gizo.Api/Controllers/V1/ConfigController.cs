using Gizo.Application.Configs.Dtos;
using Gizo.Application.Configs.QueryHandlers;

namespace Gizo.Api.Controllers.V1;

[Authorize]
public class ConfigController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<GetConfigsResponse>> Get(CancellationToken cancellationToken = default)
    {
        var query = new GetConfigsQuery();

        var result = await Mediator.Send(query, cancellationToken);
        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }
}
