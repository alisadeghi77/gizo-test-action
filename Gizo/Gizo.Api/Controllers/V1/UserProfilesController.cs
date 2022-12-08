namespace Gizo.Api.Controllers.V1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserProfilesController : BaseController
{

    [HttpGet]
    public async Task<IActionResult> GetAllProfiles(CancellationToken token)
    {
        var query = new GetAllUserProfilesQuery();
        var response = await _mediator.Send(query, token);
        var profiles = _mapper.Map<List<UserProfileResponse>>(response.Data);
        return Ok(profiles);
    }

    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [HttpGet]
    public async Task<IActionResult> GetUserProfileById(long id, CancellationToken token)
    {
        var query = new GetUserProfileByIdQuery { UserProfileId = id };
        var response = await _mediator.Send(query, token);

        if (response.IsError)
            return HandleErrorResponse(response.Errors);

        var userProfile = _mapper.Map<UserProfileResponse>(response.Data);
        return Ok(userProfile);
    }

    [HttpPatch]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [ValidateModel]
    public async Task<IActionResult> UpdateUserProfile(long id, UserProfileCreateUpdateRequest updatedProfile, 
        CancellationToken token)
    {
        var command = _mapper.Map<UpdateUserProfileBasicInfoCommand>(updatedProfile);
        command.UserProfileId = id;
        var response = await _mediator.Send(command, token);

        return response.IsError ? HandleErrorResponse(response.Errors) : NoContent();
    }
}
