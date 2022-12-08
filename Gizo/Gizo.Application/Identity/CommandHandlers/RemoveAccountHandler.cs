using Gizo.Application.Enums;
using Gizo.Application.Identity.Commands;
using Gizo.Application.Models;
using Gizo.Application.UserProfiles;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Identity.CommandHandlers;

public class RemoveAccountHandler : IRequestHandler<RemoveAccountCommand, OperationResult<bool>>
{
    private readonly DataContext _ctx;

    public RemoveAccountHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<bool>> Handle(RemoveAccountCommand request, 
        CancellationToken token)
    {
        var result = new OperationResult<bool>();

        try
        {
            var identityUser = await _ctx.Users.FirstOrDefaultAsync(iu 
                => iu.Id == request.IdentityUserId.ToString(), token);

            if (identityUser == null)
            {
                result.AddError(ErrorCode.IdentityUserDoesNotExist, 
                    IdentityErrorMessages.NonExistentIdentityUser);
                return result;
            }

            var userProfile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(up
                    => up.IdentityId == request.IdentityUserId.ToString(), token);

            if (userProfile == null)
            {
                result.AddError(ErrorCode.NotFound, UserProfilesErrorMessages.UserProfileNotFound);
                return result;
            }

            if (identityUser.Id != request.RequestorGuid.ToString())
            {
                result.AddError(ErrorCode.UnauthorizedAccountRemoval, 
                    IdentityErrorMessages.UnauthorizedAccountRemoval);

                return result;
            }

            _ctx.UserProfiles.Remove(userProfile);
            _ctx.Users.Remove(identityUser);
            await _ctx.SaveChangesAsync(token);

            result.Data = true;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}