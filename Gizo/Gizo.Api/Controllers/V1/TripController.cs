using Gizo.Api.Contracts.Trips;
using Gizo.Application.Trips.CommandHandlers;
using Gizo.Application.Trips.Dtos;
using Gizo.Application.Trips.QueryHandlers;

namespace Gizo.Api.Controllers.V1;

[Authorize]
public class TripController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<GetUserTripResponse>>> GetTrips(CancellationToken token)
    {
        var userId = HttpContext.GetIdentityIdClaimValue();
        var query = new GetAllTripsQuery(userId);

        var result = await _mediator.Send(query, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpGet]
    [Route(ApiRoutes.Trip.FileChunkStatus)]
    public async Task<ActionResult<FileChunkStatusResponse>> GetFileChunkStatus([FromQuery] FileChunkStatusRequest request,
        CancellationToken token)
    {
        var userId = HttpContext.GetIdentityIdClaimValue();
        var query = new GetFileChunkQuery(userId, request.TripId, request.TripFileType);

        var result = await _mediator.Send(query, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost]
    [Route(ApiRoutes.Trip.UploadStart)]
    public async Task<ActionResult<CreatedTripResponse>> UploadStart(CancellationToken token)
    {
        var userId = HttpContext.GetIdentityIdClaimValue();
        var command = new CreateTripCommand(userId);

        var result = await _mediator.Send(command, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost]
    [ValidateModel]
    [Route(ApiRoutes.Trip.UploadChunks)]
    public async Task<ActionResult<TripTempFileCreatedResponse>> UploadChunks([FromForm] UploadChunkRequest request,
        CancellationToken token)
    {
        var command = new UploadFileChunkCommand(request.TripId,
            request.FileChunkId,
            request.TripFileType,
            request.FileChunk.ContentType,
            request.FileChunk.OpenReadStream());

        var uploadFileResult = await _mediator.Send(command, token);

        if (uploadFileResult.IsError)
            return HandleErrorResponse(uploadFileResult.Errors);

        return Ok(uploadFileResult.Data);
    }

    [HttpPost]
    [ValidateModel]
    [Route(ApiRoutes.Trip.UploadComplete)]
    public async Task<ActionResult<bool>> UploadComplete([FromQuery] UploadCompletedRequest request,
        CancellationToken token)
    {
        var command = new UploadFileCompletedCommand(request.TripId, request.TripFileType, request.ChunkLenght);

        var result = await _mediator.Send(command, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }
}
