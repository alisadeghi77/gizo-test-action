using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Users.Commands;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.Users.CommandHandlers;

public class UpdateUserProfileBasicInfoCommandHandler 
    : IRequestHandler<UpdateUserProfileBasicInfoCommand, OperationResult<bool>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<User> _userManager;
    private OperationResult<bool> _result = new();

    public UpdateUserProfileBasicInfoCommandHandler(DataContext ctx, UserManager<User> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }

    public async Task<OperationResult<bool>> Handle(UpdateUserProfileBasicInfoCommand request, CancellationToken token)
    {
        try
        {
            var user = await UpdateUserProfile(request, token);

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

    private async Task<User> UpdateUserProfile(UpdateUserProfileBasicInfoCommand request, CancellationToken token)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
            _result.AddError(ErrorCode.IdentityUserDoesNotExist,
                UserErrorMessages.NonExistentIdentityUser);

        user.UpdateUserProfile(request.FirstName, request.LastName, request.Email);

        _ctx.Users.Update(user);
        await _ctx.SaveChangesAsync(token);

        return user;
    }
}
