using Gizo.Api.Contracts.UserCarModels.Requests;
using Gizo.Api.Contracts.UserProfile.Requests;
using Gizo.Api.Contracts.Users.Requests;
using Gizo.Application.Users.CommandHandlers;
using Gizo.Application.Users.Dtos;
using Gizo.Application.Users.QueryHandlers;

namespace Gizo.Api.Controllers.V1;

public class UserController : BaseController
{
    [HttpPost]
    [Route(ApiRoutes.User.CheckIdentity)]
    [ValidateModel]
    public async Task<ActionResult<bool>> CheckIdentity(CheckIdentityRequest checkIdentity,
        CancellationToken token)
    {
        var command = _mapper.Map<CheckClientIdentityCommand>(checkIdentity);
        var result = await _mediator.Send(command, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost]
    [Route(ApiRoutes.User.Verify)]
    [ValidateModel]
    public async Task<ActionResult<UserVerifyResponse>> Verify(VerifyIdentityRequest login,
        CancellationToken token)
    {
        var command = _mapper.Map<VerifyCommand>(login);
        var result = await _mediator.Send(command, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPatch]
    [Route(ApiRoutes.User.Profile)]
    [ValidateModel]
    [Authorize]
    public async Task<ActionResult<UserProfileResponse>> UpdateProfile(UserProfileUpdateRequest request,
        CancellationToken token)
    {
        var command = new UpdateUserProfileCommand(CurrentUserId,
            request.FirstName,
            request.LastName,
            request.Email);

        var response = await _mediator.Send(command, token);

        if (response.IsError)
            return HandleErrorResponse(response.Errors);

        return Ok(response.Data);
    }

    [HttpPost]
    [Route(ApiRoutes.User.Registration)]
    [ValidateModel]
    public async Task<IActionResult> Register(UserRegistrationRequest registration, CancellationToken token)
    {
        var command = _mapper.Map<RegisterIdentityCommand>(registration);
        var result = await _mediator.Send(command, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost]
    [Route(ApiRoutes.User.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login(LoginRequest login, CancellationToken token)
    {
        var command = _mapper.Map<LoginCommand>(login);
        var result = await _mediator.Send(command, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpDelete]
    [Route(ApiRoutes.User.Id)]
    [Authorize]
    public async Task<IActionResult> DeleteAccount(long id, CancellationToken token)
    {
        var command = new RemoveAccountCommand(id, HttpContext.GetIdentityIdClaimValue());

        var result = await _mediator.Send(command, token);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        return NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.User.CurrentUser)]
    [Authorize]
    public async Task<ActionResult<UserProfileResponse>> CurrentUser(CancellationToken token)
    {
        var query = new GetCurrentUserQuery(CurrentUserId, HttpContext.User);
        var result = await _mediator.Send(query, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpGet]
    [Route(ApiRoutes.User.CarModels)]
    [Authorize]
    public async Task<ActionResult<List<UserCarResponse>>> GetUserCarModels(CancellationToken token)
    {
        var query = new GetUserCarModelsQuery(CurrentUserId);

        var result = await _mediator.Send(query, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpGet]
    [Route(ApiRoutes.User.CarModelDetail)]
    [Authorize]
    public async Task<ActionResult<List<UserCarResponse>>> GetUserCarModelDetails(long id, CancellationToken token)
    {
        var query = new GetUserCarModelDetailsQuery(id, CurrentUserId);

        var result = await _mediator.Send(query, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost]
    [Route(ApiRoutes.User.CarModel)]
    [Authorize]
    public async Task<ActionResult<bool>> AddUserCarModel(AddUserCarModelRequest request, CancellationToken token)
    {
        var command = new AddUserCarModelCommand(request.CarModelId, CurrentUserId, request.License);

        var result = await _mediator.Send(command, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPut]
    [Route(ApiRoutes.User.CarModel)]
    [Authorize]
    public async Task<ActionResult<bool>> EditUserCarModel(EditUserCarModelRequest request, 
        CancellationToken cancellationToken = default)
    {
        var command = new EditUserCarModelCommand(request.Id, CurrentUserId, request.Licence);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpDelete]
    [Route(ApiRoutes.User.CarModel)]
    [Authorize]
    public async Task<ActionResult<bool>> DeleteUserCarModel(DeleteUserCarModelRequest request, 
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteUserCarModelCommand(request.Id, CurrentUserId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPatch]
    [Route(ApiRoutes.User.CarModelSelect)]
    [Authorize]
    public async Task<ActionResult<bool>> SelectUserCarModel(SelectUserCarModelRequest request, 
        CancellationToken cancellationToken = default)
    {
        var command = new SelectUserCarModelCommand(request.Id, CurrentUserId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }
}
