using Gizo.Api.Contracts.Trips.Requests;
using Gizo.Application.Trips.CommandHandlers;
using Gizo.Application.Trips.Dtos;
using Gizo.Application.Trips.QueryHandlers;

namespace Gizo.Api.Controllers.V1;

[Authorize]
public class TripController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<TripResponse>>> GetAll(CancellationToken token)
    {
        var query = new GetTripsQuery(CurrentUserId);

        var result = await _mediator.Send(query, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpGet]
    [Route(ApiRoutes.Trip.TripDetail)]
    public async Task<ActionResult<List<TripDetailResponse>>> Get([FromRoute] long id,
        CancellationToken token)
    {
        var query = new GetTripQuery(id, CurrentUserId);

        var result = await _mediator.Send(query, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpGet]
    [Route(ApiRoutes.Trip.FileChunkStatus)]
    public async Task<ActionResult<FileChunkStatusResponse>> GetFileChunkStatus(
        [FromQuery] FileChunkStatusRequest request,
        CancellationToken token)
    {
        var query = new GetFileChunkQuery(CurrentUserId, request.TripId, request.TripFileType);

        var result = await _mediator.Send(query, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost]
    [Route(ApiRoutes.Trip.CreateTrip)]
    public async Task<ActionResult<CreatedTripResponse>> CreateTrip(CancellationToken token)
    {
        var command = new CreateTripCommand(CurrentUserId);

        var result = await _mediator.Send(command, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }


    [HttpPost]
    [ValidateModel]
    [Route(ApiRoutes.Trip.UploadStart)]
    public async Task<ActionResult<CreatedTripResponse>> UploadStart([FromQuery] UploadStartRequest request,
        CancellationToken token)
    {
        var command = new UploadFileStartCommand(request.TripId,
            CurrentUserId,
            request.ChunkCount,
            request.TripFileType);

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
}