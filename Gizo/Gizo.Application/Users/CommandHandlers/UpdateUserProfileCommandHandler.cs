using AutoMapper;
using Gizo.Application.Enums;
using Gizo.Application.Models;
using Gizo.Application.Users.Dtos;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.Users.CommandHandlers;

public sealed record UpdateUserProfileCommand(long Id,
    string FirstName,
    string LastName,
    string Email) : IRequest<OperationResult<UserProfileResponse>>;

public class UpdateUserProfileCommandHandler
    : IRequestHandler<UpdateUserProfileCommand, OperationResult<UserProfileResponse>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly OperationResult<UserProfileResponse> _result;

    public UpdateUserProfileCommandHandler(
        DataContext ctx,
        IMapper mapper,
        UserManager<User> userManager)
    {
        _result = new();
        _ctx = ctx;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<OperationResult<UserProfileResponse>> Handle(UpdateUserProfileCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await UpdateUserProfile(request, cancellationToken);

            if (_result.IsError || user is null)
                return _result;

            _result.Data = _mapper.Map<UserProfileResponse>(user);

            return _result;
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private async Task<User?> UpdateUserProfile(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
        {
            _result.AddError(ErrorCode.IdentityUserDoesNotExist,
                UserErrorMessages.NonExistentIdentityUser);

            return null;
        }

        user.UpdateProfile(request.Id, request.FirstName, request.LastName, request.Email);

        _ctx.Users.Update(user);
        await _ctx.SaveChangesAsync(cancellationToken);

        return user;
    }
}
