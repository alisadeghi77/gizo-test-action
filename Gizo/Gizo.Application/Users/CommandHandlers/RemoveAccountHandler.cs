using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Users.CommandHandlers;

public sealed record RemoveAccountCommand(long IdentityUserId,
    long RequestorGuid) : IRequest<OperationResult<bool>>;

public class RemoveAccountHandler : IRequestHandler<RemoveAccountCommand, OperationResult<bool>>
{
    private readonly DataContext _ctx;

    public RemoveAccountHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<bool>> Handle(RemoveAccountCommand request,
        CancellationToken cancellationToken)
    {
        var result = new OperationResult<bool>();

        try
        {
            var identityUser = await _ctx.Users
                .FirstOrDefaultAsync(iu => iu.Id == request.IdentityUserId, cancellationToken);

            if (identityUser == null)
            {
                result.AddError(ErrorCode.IdentityUserDoesNotExist,
                    UserErrorMessages.NonExistentIdentityUser);
                return result;
            }

            if (identityUser.Id != request.RequestorGuid)
            {
                result.AddError(ErrorCode.UnauthorizedAccountRemoval,
                    UserErrorMessages.UnauthorizedAccountRemoval);

                return result;
            }

            _ctx.Users.Remove(identityUser);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Data = true;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}
