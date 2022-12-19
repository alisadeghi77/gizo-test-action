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
using Microsoft.EntityFrameworkCore.Storage;

namespace Gizo.Application.Users.CommandHandlers;

public class RegisterIdentityHandler : IRequestHandler<RegisterIdentityCommand, OperationResult<CurrentUserDto>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<User> _userManager;
    private readonly IdentityService _identityService;
    private OperationResult<CurrentUserDto> _result = new();
    private readonly IMapper _mapper;

    public RegisterIdentityHandler(DataContext ctx, UserManager<User> userManager,
        IdentityService identityService, IMapper mapper)
    {
        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
        _mapper = mapper;
    }
    
    public async Task<OperationResult<CurrentUserDto>> Handle(RegisterIdentityCommand request, 
        CancellationToken token)
    {
        try
        {
            await ValidateIdentityDoesNotExist(request);
            if (_result.IsError) return _result;
            
            await using var transaction = await _ctx.Database.BeginTransactionAsync(token);
            
            var identity = await CreateIdentityUserAsync(request, transaction, token);
            if (_result.IsError) return _result;

            await transaction.CommitAsync(token);

            _result.Data.UserName = identity.UserName;
            _result.Data.Token = _identityService.GetJwtString(identity);
            return _result;
        }
        
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private async Task ValidateIdentityDoesNotExist(RegisterIdentityCommand request)
    {
        var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

        if (existingIdentity != null) 
            _result.AddError(ErrorCode.IdentityUserAlreadyExists, UserErrorMessages.IdentityUserAlreadyExists);
        
    }

    private async Task<User> CreateIdentityUserAsync(RegisterIdentityCommand request, 
        IDbContextTransaction transaction, CancellationToken token)
    {
        var identity = new User() {Email = request.Username, UserName = request.Username};
        var createdIdentity = await _userManager.CreateAsync(identity, request.Password);
        if (!createdIdentity.Succeeded)
        {
            await transaction.RollbackAsync(token);

            foreach (var identityError in createdIdentity.Errors)
            {
                _result.AddError(ErrorCode.IdentityCreationFailed, identityError.Description);
            }
        }
        return identity;
    }
}