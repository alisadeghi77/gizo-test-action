namespace Gizo.Api.Controllers.V1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserProfilesController : BaseController
{

    [HttpGet]
    public async Task<IActionResult> GetAllProfiles(CancellationToken cancellationToken)
    {
        var query = new GetAllUserProfilesQuery();
        var response = await _mediator.Send(query, cancellationToken);
        var profiles = _mapper.Map<List<UserProfileResponse>>(response.Data);
        return Ok(profiles);
    }

    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [HttpGet]
    public async Task<IActionResult> GetUserProfileById(long id, CancellationToken cancellationToken)
    {
        var query = new GetUserProfileByIdQuery { UserProfileId = id };
        var response = await _mediator.Send(query, cancellationToken);

        if (response.IsError)
            return HandleErrorResponse(response.Errors);

        var userProfile = _mapper.Map<UserProfileResponse>(response.Data);
        return Ok(userProfile);
    }

    [HttpPatch]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [ValidateModel]
    public async Task<IActionResult> UpdateUserProfile(long id, UserProfileCreateUpdateRequest updatedProfile, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateUserProfileBasicInfoCommand>(updatedProfile);
        command.UserProfileId = id;
        var response = await _mediator.Send(command, cancellationToken);

        return response.IsError ? HandleErrorResponse(response.Errors) : NoContent();
    }
}
