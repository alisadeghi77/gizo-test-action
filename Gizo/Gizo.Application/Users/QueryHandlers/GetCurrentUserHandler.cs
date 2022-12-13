using AutoMapper;
using Gizo.Application.Models;
using Gizo.Application.Users.Dtos;
using Gizo.Application.Users.Queries;
using Gizo.Domain.Aggregates.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.Users.QueryHandlers;

public class GetCurrentUserHandler 
    : IRequestHandler<GetCurrentUserQuery, OperationResult<IdentityUserProfileDto>>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public GetCurrentUserHandler(
        UserManager<User> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<OperationResult<IdentityUserProfileDto>> Handle(GetCurrentUserQuery request, 
        CancellationToken token)
    {
        var result = new OperationResult<IdentityUserProfileDto>();
        var user = await _userManager.GetUserAsync(request.ClaimsPrincipal);

        result.Data.UserName = user.UserName;
        return result;
    }
}