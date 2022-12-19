using Gizo.Api.Contracts.UserProfile.Requests;
using Gizo.Api.Contracts.Users;
using Gizo.Api.Contracts.Users.Requests;
using Gizo.Application.Users.Commands;
using Gizo.Application.Users.Queries;

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
    public async Task<ActionResult<VerifyIdentityResponse>> Verify(VerifyIdentityRequest login,
        CancellationToken token)
    {
        var command = _mapper.Map<VerifyCommand>(login);
        var result = await _mediator.Send(command, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(_mapper.Map<VerifyIdentityResponse>(result.Data));
    }

    [HttpPatch]
    [Route(ApiRoutes.User.Profile)]
    [ValidateModel]
    [Authorize]
    public async Task<ActionResult<UserProfileResponse>> UpdateProfile(
        UserProfileUpdateRequest updatedProfile,
        CancellationToken token)
    {
        var command = _mapper.Map<UpdateUserProfileCommand>(updatedProfile);
        command.Id = HttpContext.GetIdentityIdClaimValue();

        var response = await _mediator.Send(command, token);

        if (response.IsError)
            return HandleErrorResponse(response.Errors);

        return Ok(_mapper.Map<UserProfileResponse>(response.Data));
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

        return Ok(_mapper.Map<CurrentUserResponse>(result.Data));
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

        return Ok(_mapper.Map<CurrentUserResponse>(result.Data));
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
        var userId = HttpContext.GetIdentityIdClaimValue();

        var query = new GetCurrentUserQuery(userId, HttpContext.User);
        var result = await _mediator.Send(query, token);

        if (result.IsError) 
            return HandleErrorResponse(result.Errors);

        return Ok(_mapper.Map<UserProfileResponse>(result.Data));
    }
}