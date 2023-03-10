namespace Gizo.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class BaseController : ControllerBase
{
    public long CurrentUserId => HttpContext.GetIdentityIdClaimValue();

    private IMediator _mediatorInstance;
    private IMapper _mapperInstance;
    protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected IMapper Mapper => _mapperInstance ??= HttpContext.RequestServices.GetRequiredService<IMapper>();

    protected ActionResult HandleErrorResponse(IReadOnlyCollection<Error> errors)
    {
        var apiError = new ErrorResponse();

        if (errors.Any(e => e.Code == ErrorCode.NotFound))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound);

            apiError.StatusCode = 404;
            apiError.StatusPhrase = "Not Found";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error?.Message);

            return NotFound(apiError);
        }

        apiError.StatusCode = 400;
        apiError.StatusPhrase = "Bad request";
        apiError.Timestamp = DateTime.Now;
        errors.ToList().ForEach(e => apiError.Errors.Add(e.Message));
        return StatusCode(400, apiError);
    }
}
