using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Gizo.Domain.Aggregates.UserProfileAggregate;
using Gizo.Application.Enums;
using Gizo.Application.Identity.Commands;
using Gizo.Application.Identity.Dtos;
using Gizo.Application.Models;
using Gizo.Application.Services;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Identity.CommandHandlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<IdentityUserProfileDto>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    private readonly IMapper _mapper;
    private OperationResult<IdentityUserProfileDto> _result = new();

    public LoginCommandHandler(DataContext ctx, UserManager<IdentityUser> userManager, 
        IdentityService identityService, IMapper mapper)
    {
        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<OperationResult<IdentityUserProfileDto>> Handle(LoginCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var identityUser = await ValidateAndGetIdentityAsync(request);
            if (_result.IsError) return _result;

            var userProfile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(up => up.IdentityId == identityUser.Id, cancellationToken:
                    cancellationToken);


            _result.Data = _mapper.Map<IdentityUserProfileDto>(userProfile);
            _result.Data.UserName = identityUser.UserName;
            _result.Data.Token = GetJwtString(identityUser, userProfile);
            return _result;

        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private async Task<IdentityUser> ValidateAndGetIdentityAsync(LoginCommand request)
    {
        var identityUser = await _userManager.FindByEmailAsync(request.Username);
            
        if (identityUser is null)
            _result.AddError(ErrorCode.IdentityUserDoesNotExist, 
                IdentityErrorMessages.NonExistentIdentityUser);

        var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

        if (!validPassword)
            _result.AddError(ErrorCode.IncorrectPassword, IdentityErrorMessages.IncorrectPassword);

        return identityUser;
    }

    private string GetJwtString(IdentityUser identityUser, UserProfile userProfile)
    {
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
            new Claim("IdentityId", identityUser.Id),
            new Claim("UserProfileId", userProfile.Id.ToString())
        });

        var token = _identityService.CreateSecurityToken(claimsIdentity);
        return _identityService.WriteToken(token);
    }
}