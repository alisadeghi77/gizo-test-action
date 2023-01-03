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
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CheckClientIdentityCommand>(checkIdentity);
        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost]
    [Route(ApiRoutes.User.Verify)]
    [ValidateModel]
    public async Task<ActionResult<UserVerifyResponse>> Verify(VerifyIdentityRequest login,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map<VerifyCommand>(login);
        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPatch]
    [Route(ApiRoutes.User.Profile)]
    [ValidateModel]
    [Authorize]
    public async Task<ActionResult<UserProfileResponse>> UpdateProfile(UserProfileUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserProfileCommand(CurrentUserId,
            request.FirstName,
            request.LastName,
            request.Email);

        var response = await Mediator.Send(command, cancellationToken);

        if (response.IsError)
            return HandleErrorResponse(response.Errors);

        return Ok(response.Data);
    }

    [HttpPost]
    [Route(ApiRoutes.User.Registration)]
    [ValidateModel]
    public async Task<IActionResult> Register(UserRegistrationRequest registration, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<RegisterIdentityCommand>(registration);
        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost]
    [Route(ApiRoutes.User.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login(LoginRequest login, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<LoginCommand>(login);
        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpDelete]
    [Route(ApiRoutes.User.Id)]
    [Authorize]
    public async Task<IActionResult> DeleteAccount(long id, CancellationToken cancellationToken)
    {
        var command = new RemoveAccountCommand(id, HttpContext.GetIdentityIdClaimValue());

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        return NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.User.CurrentUser)]
    [Authorize]
    public async Task<ActionResult<UserProfileResponse>> CurrentUser(CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserQuery(CurrentUserId, HttpContext.User);
        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpGet]
    [Route(ApiRoutes.User.CarModels)]
    [Authorize]
    public async Task<ActionResult<List<UserCarResponse>>> GetUserCarModels(CancellationToken cancellationToken)
    {
        var query = new GetUserCarModelsQuery(CurrentUserId);

        var result = await Mediator.Send(query, cancellationToken);

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

        var result = await Mediator.Send(query, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost]
    [Route(ApiRoutes.User.CarModel)]
    [Authorize]
    public async Task<ActionResult<bool>> AddUserCarModel(AddUserCarModelRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddUserCarModelCommand(request.CarModelId, CurrentUserId, request.License);

        var result = await Mediator.Send(command, cancellationToken);

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

        var result = await Mediator.Send(command, cancellationToken);

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
        var result = await Mediator.Send(command, cancellationToken);

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
        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }
}
