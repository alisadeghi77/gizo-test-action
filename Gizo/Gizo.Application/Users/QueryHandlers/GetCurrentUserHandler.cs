using AutoMapper;
using Gizo.Application.Models;
using Gizo.Application.Users.Dtos;
using Gizo.Application.Users.Queries;
using Gizo.Domain.Aggregates.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gizo.Application.Users.QueryHandlers;

public class GetCurrentUserHandler
    : IRequestHandler<GetCurrentUserQuery, OperationResult<UserProfileDto>>
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

    public async Task<OperationResult<UserProfileDto>> Handle(GetCurrentUserQuery request,
        CancellationToken token)
    {
        var result = new OperationResult<UserProfileDto>();
        var user = await _userManager.FindByIdAsync(request.userId.ToString());

        result.Data = _mapper.Map<UserProfileDto>(user);
        return result;
    }
}