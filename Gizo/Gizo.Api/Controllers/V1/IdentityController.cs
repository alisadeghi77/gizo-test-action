using Gizo.Application.Identity.Queries;

namespace Gizo.Api.Controllers.V1;

public class IdentityController : BaseController
{
    [HttpPost]
    [Route(ApiRoutes.Identity.Registration)]
    [ValidateModel]
    public async Task<IActionResult> Register(UserRegistrationRequest registration, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterIdentityCommand>(registration);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        return Ok(_mapper.Map<IdentityUserProfile>(result.Data));
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login(LoginRequest login, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<LoginCommand>(login);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(_mapper.Map<IdentityUserProfile>(result.Data));
    }

    [HttpDelete]
    [Route(ApiRoutes.Identity.IdentityById)]
    [Authorize]
    public async Task<IActionResult> DeleteAccount(long identityUserId, CancellationToken token)
    {
        var command = new RemoveAccountCommand
        {
            IdentityUserId = identityUserId,
            RequestorGuid = HttpContext.GetIdentityIdClaimValue()
        };
        var result = await _mediator.Send(command, token);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        return NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Identity.CurrentUser)]
    [Authorize]
    public async Task<IActionResult> CurrentUser(CancellationToken token)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var query = new GetCurrentUserQuery { UserProfileId = userProfileId, ClaimsPrincipal = HttpContext.User};
        var result = await _mediator.Send(query, token);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        return Ok(_mapper.Map<IdentityUserProfile>(result.Data));
    }
}