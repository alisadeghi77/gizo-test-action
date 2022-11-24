namespace Gizo.Api.Filters;

public class GizoExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var apiError = new ErrorResponse
        {
            StatusCode = 500,
            StatusPhrase = "Internal Server Error",
            Timestamp = DateTime.Now
        };
        apiError.Errors.Add(context.Exception.Message);

        context.Result = new JsonResult(apiError) { StatusCode = 500};
    }
}