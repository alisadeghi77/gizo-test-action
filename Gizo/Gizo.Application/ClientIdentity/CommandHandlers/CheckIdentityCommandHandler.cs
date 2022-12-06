using Gizo.Application.ClientIdentity.Commands;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.ClientIdentity.CommandHandlers;

public class CheckIdentityCommandHandler : IRequestHandler<CheckClientIdentityCommand, OperationResult<bool>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private OperationResult<bool> _result = new();

    public CheckIdentityCommandHandler(DataContext ctx, UserManager<IdentityUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }

    public async Task<OperationResult<bool>> Handle(CheckClientIdentityCommand request, CancellationToken token)
    {
        try
        {
            var identityUser = await ValidateAndGetIdentityAsync(request, token);

            if (_result.IsError) 
                return _result;

            _result.Data = true;
            return _result;
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private async Task<IdentityUser> ValidateAndGetIdentityAsync(CheckClientIdentityCommand request, CancellationToken token)
    {
        var identityUser = await _userManager.FindByNameAsync(request.Username);

        if (identityUser == null)
            identityUser = await CreateIdentityUserAsync(request, token);

        return identityUser;
    }

    private async Task<IdentityUser> CreateIdentityUserAsync(CheckClientIdentityCommand request,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);
        var identity = new IdentityUser { PhoneNumber = request.Username, UserName = request.Username };
        var createdIdentity = await _userManager.CreateAsync(identity);

        if (!createdIdentity.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);

            foreach (var identityError in createdIdentity.Errors)
            {
                _result.AddError(ErrorCode.IdentityCreationFailed, identityError.Description);
            }
        }

        await transaction.CommitAsync(cancellationToken);

        return identity;
    }
}
