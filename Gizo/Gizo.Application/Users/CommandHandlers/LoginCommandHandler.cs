using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Services;
using Gizo.Application.Users.Dtos;
using Gizo.Domain.Aggregates.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.Users.CommandHandlers;

public sealed record LoginCommand(
    string Username,
    string Password) : IRequest<OperationResult<CurrentUserResponse>>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<CurrentUserResponse>>
{
    private readonly UserManager<User> _userManager;
    private readonly IdentityService _identityService;
    private readonly OperationResult<CurrentUserResponse> _result;

    public LoginCommandHandler(UserManager<User> userManager,
        IdentityService identityService)
    {
        _result = new();
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<OperationResult<CurrentUserResponse>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var identityUser = await ValidateAndGetIdentityAsync(request);

            if (_result.IsError || identityUser is null)
                return _result;

            _result.Data ??= new CurrentUserResponse();
            _result.Data.UserName = identityUser.UserName;
            _result.Data.Token = _identityService.GetJwtString(identityUser);

            return _result;
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private async Task<User?> ValidateAndGetIdentityAsync(LoginCommand request)
    {
        var identityUser = await _userManager.FindByEmailAsync(request.Username);

        if (identityUser is null)
        {
            _result.AddError(ErrorCode.IdentityUserDoesNotExist, UserErrorMessages.NonExistentIdentityUser);
            return null;
        }

        var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

        if (!validPassword)
            _result.AddError(ErrorCode.IncorrectPassword, UserErrorMessages.IncorrectPassword);

        return identityUser;
    }
}
