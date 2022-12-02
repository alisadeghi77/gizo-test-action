﻿using Gizo.Application.ClientIdentity.Dtos;
using Gizo.Application.Enums;
using Gizo.Application.Identity;
using Gizo.Application.Models;
using Gizo.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.ClientIdentity.Commands;

public class VerifyCommandHandler : IRequestHandler<VerifyClientIdentityCommand, OperationResult<ClientIdentityUserDto>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    private OperationResult<ClientIdentityUserDto> _result = new();

    public VerifyCommandHandler(UserManager<IdentityUser> userManager,
        IdentityService identityService)
    {
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<OperationResult<ClientIdentityUserDto>> Handle(VerifyClientIdentityCommand request, CancellationToken token)
    {
        try
        {
            var identityUser = await ValidateAndGetIdentityAsync(request);
            if (_result.IsError) 
                return _result;

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

    private async Task<IdentityUser> ValidateAndGetIdentityAsync(VerifyClientIdentityCommand request)
    {
        var identityUser = await _userManager.FindByEmailAsync(request.Username);

        if (identityUser is null)
            _result.AddError(ErrorCode.IdentityUserDoesNotExist,
                ClientIdentityErrorMessages.NonExistentIdentityUser);

        // TODO validVerifyCode 

        if (request.VerifyCode != "11111")
            _result.AddError(ErrorCode.IncorrectPassword, ClientIdentityErrorMessages.InvalidVerificationoCode);

        return identityUser;
    }
}