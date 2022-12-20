using Gizo.Api.Contracts.Trips;
using Gizo.Application.Trips.Commands;

namespace Gizo.Api.Controllers.V1;

[Authorize]
public class TripController : BaseController
{
    private readonly IWebHostEnvironment _environment;

    public TripController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpPost]
    [Route(ApiRoutes.Trip.TripStart)]
    public async Task<IActionResult> TripStart(CancellationToken token)
    {
        var userId = HttpContext.GetIdentityIdClaimValue();

        var command = new CreateTripCommand(
            userId,
            _environment.WebRootPath);

        var result = await _mediator.Send(command, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost]
    [ValidateModel]
    [Route(ApiRoutes.Trip.UploadChunks)]
    public async Task<IActionResult> UploadChunks(
        [FromForm] UploadChunkRequest fileChunk,
        CancellationToken token)
    {
        try
        {
            var command = new CreateTripTempVideoCommand(
                fileChunk.TripId,
                fileChunk.FileChunkId,
                fileChunk.FileName,
                fileChunk.FileChunk.OpenReadStream());

            var createVideoFileResult = await _mediator.Send(command, token);

            if (createVideoFileResult.IsError)
                return HandleErrorResponse(createVideoFileResult.Errors);

            return Ok(createVideoFileResult);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost]
    [ValidateModel]
    [Route(ApiRoutes.Trip.UploadComplete)]
    public async Task<IActionResult> UploadComplete(
        [FromQuery] UploadCompletedRequest uploadCompleted,
        CancellationToken token)
    {
        try
        {
            var command = new UploadVideoCompletedCommand(
                uploadCompleted.TripId,
                uploadCompleted.FileName);

            var result = await _mediator.Send(command, token);

            if (result.IsError)
                return HandleErrorResponse(result.Errors);

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
