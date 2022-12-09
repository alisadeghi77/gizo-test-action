using Gizo.Api.Contracts.ClientIdentity;
using Gizo.Api.Contracts.UserProfile.Requests;
using Gizo.Application.ClientIdentity.Commands;

namespace Gizo.Api.Controllers.V1;

public class ClientIdentityController : BaseController
{
    [HttpPost]
    [Route(ApiRoutes.ClientIdentity.CheckIdentity)]
    [ValidateModel]
    public async Task<IActionResult> CheckIdentity(CheckClientIdentityRequest checkIdentity, CancellationToken token)
    {
        var command = _mapper.Map<CheckClientIdentityCommand>(checkIdentity);
        var result = await _mediator.Send(command, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost]
    [Route(ApiRoutes.ClientIdentity.Verify)]
    [ValidateModel]
    public async Task<IActionResult> Verify(VerifyClientIdentityRequest login, CancellationToken token)
    {
        var command = _mapper.Map<VerifyClientIdentityCommand>(login);
        var result = await _mediator.Send(command, token);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(_mapper.Map<VerifyClientIdentityResult>(result.Data));
    }

    [HttpPatch]
    [Route(ApiRoutes.ClientIdentity.UpdateUserProfile)]
    [ValidateModel]
    [Authorize]
    public async Task<IActionResult> UpdateUserProfile(UserProfileUpdateRequest updatedProfile, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateUserProfileBasicInfoCommand>(updatedProfile);
        command.Id = HttpContext.GetIdentityIdClaimValue();

        var response = await _mediator.Send(command, cancellationToken);

        return response.IsError ? HandleErrorResponse(response.Errors) : NoContent();
    }
}
