using AutoMapper;
using Gizo.Application.Enums;
using Gizo.Application.Identity.Dtos;
using Gizo.Application.Identity.Queries;
using Gizo.Application.Models;
using Gizo.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gizo.Application.Identity.QueryHandlers;

public class GetCurrentUserHandler 
    : IRequestHandler<GetCurrentUser, OperationResult<IdentityUserProfileDto>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private OperationResult<IdentityUserProfileDto> _result = new();

    public GetCurrentUserHandler(DataContext ctx, UserManager<IdentityUser> userManager, IMapper mapper)
    {
        _ctx = ctx;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<OperationResult<IdentityUserProfileDto>> Handle(GetCurrentUser request, 
        CancellationToken cancellationToken)
    {
        var identity = await _userManager.GetUserAsync(request.ClaimsPrincipal);

        var profile = await _ctx.UserProfiles
            .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken);

        _result.Payload = _mapper.Map<IdentityUserProfileDto>(profile);
        _result.Payload.UserName = identity.UserName;
        return _result;
    }
}