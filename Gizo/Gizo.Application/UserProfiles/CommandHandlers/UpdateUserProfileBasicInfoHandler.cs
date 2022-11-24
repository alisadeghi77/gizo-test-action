using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.UserProfiles.Commands;
using Gizo.Infrastructure;
using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.UserProfiles.CommandHandlers;

internal class UpdateUserProfileBasicInfoHandler : IRequestHandler<UpdateUserProfileBasicInfo, OperationResult<UserProfile>>
{
    private readonly DataContext _ctx;

    public UpdateUserProfileBasicInfoHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileBasicInfo request,
        CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();

        try
        {
            var userProfile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken: cancellationToken);

            if (userProfile is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(UserProfilesErrorMessages.UserProfileNotFound, request.UserProfileId));
                return result;
            }

            var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName,
                request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);

            userProfile.UpdateBasicInfo(basicInfo);

            _ctx.UserProfiles.Update(userProfile);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Data = userProfile;
            return result;
        }

        catch (UserProfileNotValidException ex)
        {
            ex.ValidationErrors.ForEach(e => result.AddError(ErrorCode.ValidationError, e));
        }

        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}
