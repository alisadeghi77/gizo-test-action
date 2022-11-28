﻿using System.Security.Claims;
using AutoMapper;
using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Domain.Exceptions;
using Gizo.Application.Enums;
using Gizo.Application.Identity.Commands;
using Gizo.Application.Identity.Dtos;
using Gizo.Application.Models;
using Gizo.Application.Services;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Gizo.Application.Identity.CommandHandlers;

public class RegisterIdentityHandler : IRequestHandler<RegisterIdentityCommand, OperationResult<IdentityUserProfileDto>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    private OperationResult<IdentityUserProfileDto> _result = new();
    private readonly IMapper _mapper;

    public RegisterIdentityHandler(DataContext ctx, UserManager<IdentityUser> userManager,
        IdentityService identityService, IMapper mapper)
    {
        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
        _mapper = mapper;
    }
    
    public async Task<OperationResult<IdentityUserProfileDto>> Handle(RegisterIdentityCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            await ValidateIdentityDoesNotExist(request);
            if (_result.IsError) return _result;
            
            await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);
            
            var identity = await CreateIdentityUserAsync(request, transaction, cancellationToken);
            if (_result.IsError) return _result;

            var profile = await CreateUserProfileAsync(request, transaction, identity, cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            _result.Data = _mapper.Map<IdentityUserProfileDto>(profile);
            _result.Data.UserName = identity.UserName;
            _result.Data.Token = GetJwtString(identity, profile);
            return _result;
        }
        
        catch (UserProfileNotValidException ex)
        {
            ex.ValidationErrors.ForEach(e => _result.AddError(ErrorCode.ValidationError, e));
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
            _result.AddError(ErrorCode.IdentityUserAlreadyExists, IdentityErrorMessages.IdentityUserAlreadyExists);
        
    }

    private async Task<IdentityUser> CreateIdentityUserAsync(RegisterIdentityCommand request, 
        IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        var identity = new IdentityUser {Email = request.Username, UserName = request.Username};
        var createdIdentity = await _userManager.CreateAsync(identity, request.Password);
        if (!createdIdentity.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);

            foreach (var identityError in createdIdentity.Errors)
            {
                _result.AddError(ErrorCode.IdentityCreationFailed, identityError.Description);
            }
        }
        return identity;
    }

    private async Task<UserProfile> CreateUserProfileAsync(RegisterIdentityCommand request, 
        IDbContextTransaction transaction, IdentityUser identity,
        CancellationToken cancellationToken)
    {
        try
        {
            var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username,
                request.Phone, request.DateOfBirth, request.CurrentCity);

            var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);
            _ctx.UserProfiles.Add(profile);
            await _ctx.SaveChangesAsync(cancellationToken);
            return profile;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private string GetJwtString(IdentityUser identity, UserProfile profile)
    {
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, identity.Email),
            new Claim("IdentityId", identity.Id),
            new Claim("UserProfileId", profile.UserProfileId.ToString())
        });

        var token = _identityService.CreateSecurityToken(claimsIdentity);
        return _identityService.WriteToken(token);
    }
}