using AutoMapper;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Users.Commands;
using Gizo.Application.Users.Dtos;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.Users.CommandHandlers;

public class UpdateUserProfileCommandHandler 
    : IRequestHandler<UpdateUserProfileCommand, OperationResult<UserProfileDto>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private OperationResult<UserProfileDto> _result = new();

    public UpdateUserProfileCommandHandler(
        DataContext ctx,
        IMapper mapper,
        UserManager<User> userManager)
    {
        _ctx = ctx;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<OperationResult<UserProfileDto>> Handle(UpdateUserProfileCommand request, CancellationToken token)
    {
        try
        {
            var user = await UpdateUserProfile(request, token);

            if (_result.IsError)
                return _result;

            _result.Data = _mapper.Map<UserProfileDto>(user);
            return _result;
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private async Task<User> UpdateUserProfile(UpdateUserProfileCommand request, CancellationToken token)
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
