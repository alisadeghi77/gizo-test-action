using AutoMapper;
using Gizo.Application.Models;
using Gizo.Application.Users.Dtos;
using Gizo.Domain.Aggregates.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Gizo.Application.Users.QueryHandlers;

public sealed record GetCurrentUserQuery(
    long userId,
    ClaimsPrincipal ClaimsPrincipal) : IRequest<OperationResult<UserProfileResponse>>;

public class GetCurrentUserQueryHandler
    : IRequestHandler<GetCurrentUserQuery, OperationResult<UserProfileResponse>>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public GetCurrentUserQueryHandler(
        UserManager<User> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<OperationResult<UserProfileResponse>> Handle(GetCurrentUserQuery request,
        CancellationToken token)
    {
        var result = new OperationResult<UserProfileResponse>();
        var user = await _userManager.FindByIdAsync(request.userId.ToString());

        result.Data = _mapper.Map<UserProfileResponse>(user);
        return result;
    }
}