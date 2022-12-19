using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Services;
using Gizo.Application.Users.Commands;
using Gizo.Application.Users.Dtos;
using Gizo.Domain.Aggregates.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.Users.CommandHandlers;

public class VerifyCommandHandler : IRequestHandler<VerifyCommand, OperationResult<UserDto>>
{
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly IdentityService _identityService;
    private OperationResult<UserDto> _result = new();

    public VerifyCommandHandler(UserManager<Domain.Aggregates.UserAggregate.User> userManager,
        IdentityService identityService)
    {
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<OperationResult<UserDto>> Handle(VerifyCommand request,
        CancellationToken token)
    {
        try
        {
            var identityUser = await ValidateAndGetIdentityAsync(request);
            if (_result.IsError)
                return _result;

            _result.Data = new UserDto()
            {
                UserName = identityUser.UserName,
                Token = _identityService.GetJwtString(identityUser)
            };

            return _result;
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private async Task<User> ValidateAndGetIdentityAsync(VerifyCommand request)
    {
        var identityUser = await _userManager.FindByNameAsync(request.Username);

        if (identityUser is null)
            _result.AddError(ErrorCode.IdentityUserDoesNotExist,
                UserErrorMessages.NonExistentIdentityUser);

        // TODO validVerifyCode 

        if (request.VerifyCode != "11111")
            _result.AddError(ErrorCode.IncorrectPassword, UserErrorMessages.InvalidVerificationCode);

        return identityUser;
    }
}