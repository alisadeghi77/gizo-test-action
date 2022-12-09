using AutoMapper;
using Gizo.Application.Identity.Dtos;
using Gizo.Application.Identity.Queries;
using Gizo.Application.Models;
using Gizo.Domain.Aggregates.UserAggregate;
using Gizo.Domain.Contracts.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.Identity.QueryHandlers;

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