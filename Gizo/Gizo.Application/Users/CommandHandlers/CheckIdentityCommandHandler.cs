using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Services;
using Gizo.Application.Users.Commands;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.Users.CommandHandlers;

public class CheckIdentityCommandHandler : IRequestHandler<CheckClientIdentityCommand, OperationResult<bool>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<User> _userManager;
    private readonly IdentityService _identityService;
    private OperationResult<bool> _result = new();

    public CheckIdentityCommandHandler(DataContext ctx, UserManager<User> userManager,
        IdentityService _identityService)
    {
        _ctx = ctx;
        _userManager = userManager;
    }

    public async Task<OperationResult<bool>> Handle(CheckClientIdentityCommand request, CancellationToken token)
    {
        try
        {
            var user = await ValidateAndGetIdentityAsync(request, token);

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

    private async Task<User> ValidateAndGetIdentityAsync(CheckClientIdentityCommand request, CancellationToken token)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user == null)
            user = await CreateIdentityUserAsync(request, token);

        return user;
    }

    private async Task<User> CreateIdentityUserAsync(CheckClientIdentityCommand request,
        CancellationToken token)
    {
        await using var transaction = await _ctx.Database.BeginTransactionAsync(token);
        var identity = new User { PhoneNumber = request.Username, UserName = request.Username };

        var createdIdentity = await _userManager.CreateAsync(identity);

        if (!createdIdentity.Succeeded)
        {
            await transaction.RollbackAsync(token);

            foreach (var identityError in createdIdentity.Errors)
            {
                _result.AddError(ErrorCode.IdentityCreationFailed, identityError.Description);
            }
        }

        await transaction.CommitAsync(token);

        return identity;
    }
}
