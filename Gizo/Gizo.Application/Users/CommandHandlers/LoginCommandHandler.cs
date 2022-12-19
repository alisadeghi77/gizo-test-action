﻿using AutoMapper;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Services;
using Gizo.Application.Users.Commands;
using Gizo.Application.Users.Dtos;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.Users.CommandHandlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<CurrentUserDto>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<User> _userManager;
    private readonly IdentityService _identityService;
    private readonly IMapper _mapper;
    private OperationResult<CurrentUserDto> _result = new();

    public LoginCommandHandler(DataContext ctx, UserManager<User> userManager, 
        IdentityService identityService, IMapper mapper)
    {
        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<OperationResult<CurrentUserDto>> Handle(LoginCommand request, 
        CancellationToken token)
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

    private async Task<User> ValidateAndGetIdentityAsync(LoginCommand request)
    {
        var identityUser = await _userManager.FindByEmailAsync(request.Username);
            
        if (identityUser is null)
            _result.AddError(ErrorCode.IdentityUserDoesNotExist, 
                UserErrorMessages.NonExistentIdentityUser);

        var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

        if (!validPassword)
            _result.AddError(ErrorCode.IncorrectPassword, UserErrorMessages.IncorrectPassword);

        return identityUser;
    }
}